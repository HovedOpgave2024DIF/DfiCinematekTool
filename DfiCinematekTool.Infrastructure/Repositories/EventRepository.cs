﻿using DfiCinematekTool.Domain.Entities;
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

				//Add duration
				foreach (Film film in newEvent.Films) 
				{
					newEvent.DurationInMinutes += film.DurationInMinutes;
				}

                await _dbContext.Events.AddAsync(newEvent);
                await _dbContext.SaveChangesAsync();

                // Ret lav if
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
				throw new ArgumentOutOfRangeException(nameof(id), "ID must be greater than 0.");

			return await _dbContext.Events
				.Include(f => f.Films)
				.FirstOrDefaultAsync(ev => ev.Id == id);
		}

		public async Task<Event?> UpdateEventAsync(Event updatedEvent)
		{
			if (updatedEvent is null)
				throw new ArgumentNullException(nameof(updatedEvent), "Updated event cannot be null.");

			var eventToUpdate = await _dbContext.Events
                .Include(e => e.Films)
				.FirstOrDefaultAsync(ev => ev.Id == updatedEvent.Id);

			if (eventToUpdate is null)
				return null;

			eventToUpdate.Title = updatedEvent.Title;
			//eventToUpdate.DateId = updatedEvent.DateId;
			eventToUpdate.Date = updatedEvent.Date;
			eventToUpdate.Screen = updatedEvent.Screen;
			eventToUpdate.DurationInMinutes = updatedEvent.DurationInMinutes;
			eventToUpdate.Owner = updatedEvent.Owner;
			eventToUpdate.OwnerEmail = updatedEvent.OwnerEmail;
			eventToUpdate.EventType = updatedEvent.EventType;
			eventToUpdate.IsEvent = updatedEvent.IsEvent;
			eventToUpdate.IsRooftop = updatedEvent.IsRooftop;
			eventToUpdate.Published = updatedEvent.Published;
			eventToUpdate.Abbriviation = updatedEvent.Abbriviation;

			var currentFilmIds = eventToUpdate?.Films?.Select(f => f.Id).ToList() ?? [];
			var updatedFilmIds = updatedEvent?.Films?.Select(f => f.Id).ToList() ?? [];

            // Add Films that are new
            var filmsToAdd = updatedFilmIds.Except(currentFilmIds).ToList();
            foreach (var filmId in filmsToAdd)
            {
                // Check if the Film is already tracked
                var existingFilm = eventToUpdate.Films?.FirstOrDefault(f => f.Id == filmId);
                if (existingFilm == null)
                {
                    // Create a new Film reference without querying the database
                    var filmToAdd = new Film { Id = filmId };

                    // Attach it directly without re-querying
                    _dbContext.Entry(filmToAdd).State = EntityState.Unchanged;

                    // Add the Film reference to the event
                    eventToUpdate.Films?.Add(filmToAdd);

                    // Add FilmStatus
                    await _filmStatusRepository.CreateFilmStatusAsync(new FilmStatus
                    {
                        FilmId = filmId,
                        EventId = updatedEvent?.Id
                    });
                }
            }



            var filmsToRemove = currentFilmIds.Except(updatedFilmIds).ToList();
			foreach (var filmId in filmsToRemove)
			{
				var filmToRemove = eventToUpdate?.Films?.FirstOrDefault(f => f.Id == filmId);
				if (filmToRemove != null)
				{
					eventToUpdate?.Films?.Remove(filmToRemove);

					var filmStatus = await _filmStatusRepository.GetFilmStatusByIdsAsync(updatedEvent.Id, filmId);
					if (filmStatus != null)
					{
						await _filmStatusRepository.DeleteFilmStatusAsync(updatedEvent.Id, filmId);
					}
				}
			}

			await _dbContext.SaveChangesAsync();

			return eventToUpdate;
		}

		public async Task<bool> DeleteEventByIdAsync(int id)
		{
			if (id <= 0)
				throw new ArgumentOutOfRangeException(nameof(id), "ID must be greater than 0.");

			var eventToDelete = await _dbContext.Events.FirstOrDefaultAsync(ev => ev.Id == id);
			
			if (eventToDelete is null) return false;

			var eventFilms = eventToDelete.Films.ToList();

			if (eventFilms.Count > 0)
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
