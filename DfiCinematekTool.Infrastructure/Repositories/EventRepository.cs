using DfiCinematekTool.Domain.Entities;
using DfiCinematekTool.Domain.Interfaces;
using DfiCinematekTool.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DfiCinematekTool.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly CinematekDbContext _dbContext;
        private readonly IFilmStatusRepository _filmStatusRepository;
		public EventRepository(CinematekDbContext dbContext, IFilmStatusRepository filmStatusRepository)
        {
            _dbContext = dbContext;
            _filmStatusRepository = filmStatusRepository;
		}

		public async Task<Event> CreateEventAsync(Event newEvent)
		{
			if (newEvent is null) throw new ArgumentNullException(nameof(newEvent), "New event cannot be null.");

			if (newEvent.Films is not null)
			{
				var filmIds = newEvent.Films.Select(f => f.Id).ToList();

				var existingFilms = await _dbContext.Films.Where(f => filmIds.Contains(f.Id)).ToListAsync();

				if (existingFilms.Count != filmIds.Count)
				{
					var missingIds = filmIds.Except(existingFilms.Select(f => f.Id));
					throw new InvalidOperationException($"Films with {string.Join(",", missingIds)} does not exist.");
				}

				newEvent.Films = existingFilms;

                await _dbContext.Events.AddAsync(newEvent);
                await _dbContext.SaveChangesAsync();

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

		public async Task<List<Event>> GetAllEventsAsync()
		{
			return await _dbContext.Events.Include(f => f.Films).Include(fs => fs.FilmStatuses).ToListAsync();
		}

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

		public async Task<Event?> GetEventByIdAsync(int id)
		{
			if (id < 1)
				throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than 0.");

			return await _dbContext.Events
				.Include(f => f.Films).Include(fs => fs.FilmStatuses)
				.FirstOrDefaultAsync(ev => ev.Id == id);
		}

		public async Task<Event?> UpdateEventAsync(Event updatedEvent)
		{
			if (updatedEvent is null)
				throw new ArgumentNullException(nameof(updatedEvent), "Updated event cannot be null.");

			var eventToUpdate = await GetEventByIdAsync(updatedEvent.Id);

			if (eventToUpdate is null)
				return null;

			var currentFilmStatusFilmIds = eventToUpdate?.FilmStatuses?.Select(f => f.FilmId).ToList() ?? [];
			var updatedFilmIds = updatedEvent.Films?.Select(f => f.Id).ToList() ?? [];


			var filmStatusToCreate = updatedFilmIds.Except(currentFilmStatusFilmIds).ToList();
			foreach (var filmId in filmStatusToCreate)
			{
				await _filmStatusRepository.CreateFilmStatusAsync(new FilmStatus { EventId = eventToUpdate!.Id, FilmId = filmId });
			}

			var filmStatusToRemove = currentFilmStatusFilmIds.Except(updatedFilmIds).ToList();
			foreach (var filmId in filmStatusToRemove)
			{
				await _filmStatusRepository.DeleteFilmStatusAsync(eventToUpdate!.Id, filmId);
			}

			await _dbContext.SaveChangesAsync();

			return updatedEvent;
		}

		public async Task<bool> DeleteEventByIdAsync(int id)
		{
			if (id <= 0)
				throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than 0.");

			var eventToDelete = await _dbContext.Events.FirstOrDefaultAsync(ev => ev.Id == id);
			
			if (eventToDelete is null) return false;

			var eventFilms = eventToDelete.Films?.ToList();

			if (eventFilms?.Count > 0)
			{
				foreach (var film in eventFilms)
				{
					await _filmStatusRepository.DeleteFilmStatusAsync(eventToDelete.Id, film.Id);
				}
			}

			_dbContext.Events.Remove(eventToDelete);
			await _dbContext.SaveChangesAsync();

			return true;
		}
	}
}
