using DfiCinematekTool.Domain.Entities;
using DfiCinematekTool.Domain.Interfaces;
using DfiCinematekTool.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DfiCinematekTool.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
		#region Fields & Constructor
		private readonly CinematekDbContext _dbContext;
        private readonly IFilmStatusRepository _filmStatusRepository;

		public EventRepository(CinematekDbContext dbContext, IFilmStatusRepository filmStatusRepository)
        {
            _dbContext = dbContext;
            _filmStatusRepository = filmStatusRepository;
		}
		#endregion

		#region Get all events
		public async Task<List<Event>> GetAllEventsAsync()
		{
			return await _dbContext.Events.Include(f => f.Films).Include(fs => fs.FilmStatuses).ToListAsync();
		}
		#endregion

		#region Get event by id
		/// <summary>
		/// Finds event by id. Including film entities associated with the event.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public async Task<Event?> GetEventByIdAsync(int id)
		{
			if (id <= 0)
				throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than 0.");

			return await _dbContext.Events
				.Include(f => f.Films).Include(fs => fs.FilmStatuses)
				.FirstOrDefaultAsync(ev => ev.Id == id);
		}
		#endregion

		#region Create event

		/// <summary>
		/// Creates new Event entity and automaticly adds a new FilmStatus Entity.
		/// </summary>
		/// <param name="newEvent"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public async Task<Event> CreateEventAsync(Event newEvent)
		{
			if (newEvent is null) throw new ArgumentNullException(nameof(newEvent), "New event cannot be null.");

			if (newEvent.Films is not null)
			{
				// Creates list of film ids
				var filmIds = newEvent.Films.Select(f => f.Id).ToList();

				// Fetches the films from database
				var existingFilms = await _dbContext.Films.Where(f => filmIds.Contains(f.Id)).ToListAsync();

				if (existingFilms.Count != filmIds.Count)
				{
					var missingIds = filmIds.Except(existingFilms.Select(f => f.Id));
					throw new InvalidOperationException($"Films with {string.Join(",", missingIds)} does not exist.");
				}

				newEvent.Films = existingFilms;

                await _dbContext.Events.AddAsync(newEvent);
                await _dbContext.SaveChangesAsync();

				// Creates new film status for every film
                foreach (var film in newEvent.Films)
				{
					await _filmStatusRepository.CreateFilmStatusAsync(
						new FilmStatus
						{
							FilmId = film.Id,
							EventId = newEvent.Id
						}
					);
				}
			}
			return newEvent;
		}
		#endregion

		#region Update event

		/// <summary>
		/// Updates Event entity properties. Associated FilmStatus entities will be created or deleted.
		/// </summary>
		/// <param name="updatedEvent"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public async Task<Event?> UpdateEventAsync(Event updatedEvent)
		{
			if (updatedEvent is null)
				throw new ArgumentNullException(nameof(updatedEvent), "Updated event cannot be null.");

			var eventToUpdate = await GetEventByIdAsync(updatedEvent.Id);

			if (eventToUpdate is null)
				return null;

			var currentFilmStatusFilmIds = eventToUpdate?.FilmStatuses?.Select(f => f.FilmId).ToList() ?? [];
			var updatedFilmIds = updatedEvent.Films?.Select(f => f.Id).ToList() ?? [];

			// Creates new FilmStatus for newly added film(s) for the event
			var filmStatusToCreate = updatedFilmIds.Except(currentFilmStatusFilmIds).ToList();
			foreach (var filmId in filmStatusToCreate)
			{
				await _filmStatusRepository.CreateFilmStatusAsync(new FilmStatus { EventId = eventToUpdate!.Id, FilmId = filmId });
			}

			// Removes FilmStatus' for removed films for the event
			var filmStatusToRemove = currentFilmStatusFilmIds.Except(updatedFilmIds).ToList();
			foreach (var filmId in filmStatusToRemove)
			{
				await _filmStatusRepository.DeleteFilmStatusAsync(eventToUpdate!.Id, filmId);
			}

			await _dbContext.SaveChangesAsync();

			return updatedEvent;
		}
		#endregion

		#region Delete event
		/// <summary>
		/// Deletes the event. Associated film status will be deleted.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public async Task<bool> DeleteEventByIdAsync(int id)
		{
			if (id <= 0)
				throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than 0.");

			var eventToDelete = await _dbContext.Events.FirstOrDefaultAsync(ev => ev.Id == id);
			
			if (eventToDelete is null) return false;

			var eventFilms = eventToDelete.Films?.ToList();

			// Checks for associated films
			if (eventFilms?.Count > 0)
			{
				foreach (var film in eventFilms)
				{
					// Deletes FilmStatus' associated with the event
					await _filmStatusRepository.DeleteFilmStatusAsync(eventToDelete.Id, film.Id);
				}
			}

			_dbContext.Events.Remove(eventToDelete);
			await _dbContext.SaveChangesAsync();

			return true;
		}
		#endregion

		#region Get paginated events
		/// <summary>
		/// Fetches a fixed list of films.
		/// </summary>
		/// <param name="pageNumber"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public async Task<List<Event>> GetPaginatedEventsAsync(int pageNumber, int pageSize)
		{
			if (pageNumber < 1)
				throw new ArgumentOutOfRangeException(nameof(pageNumber),
					"Page number must be greater than or equal to 1.");

			if (pageSize < 1)
				throw new ArgumentOutOfRangeException(nameof(pageSize),
					"Page size must be greater than or equal to 1.");

			var skipNumber = (pageNumber - 1) * pageSize;

			return await _dbContext.Events
				.Include(f => f.Films)
				.Skip(skipNumber)
				.Take(pageSize)
				.ToListAsync();
		}
		#endregion
	}
}
