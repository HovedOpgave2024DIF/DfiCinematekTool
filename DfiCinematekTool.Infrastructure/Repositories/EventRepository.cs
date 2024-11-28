using DfiCinematekTool.Domain.Entities;
using DfiCinematekTool.Domain.Interfaces;
using DfiCinematekTool.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DfiCinematekTool.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly CinematekDbContext _dbContext;

        public EventRepository(CinematekDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Event> CreateEventAsync(Event newEvent)
        { 
            await _dbContext.Events.AddAsync(newEvent);
            await _dbContext.SaveChangesAsync();
            return newEvent;
        }

        public async Task<List<Event>> GetAllEventsAsync()
        {
            return await _dbContext.Events.ToListAsync();
        }

        public async Task<Event?> GetEventByIdAsync(int id)
        {
            return await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Event?> DeleteEventAsync(int id)
        {
            var eventToDelete = await _dbContext.Events.FirstOrDefaultAsync(ev => ev.Id == id);

            if (eventToDelete is not null) 
            { 
                _dbContext.Events.Remove(eventToDelete);
                await _dbContext.SaveChangesAsync();
            }
            
            return null;
        }

        public async Task<Event?> UpdateEventAsync(int id, Event updatedEvent) 
        {
            var eventToUpdate = await _dbContext.Events.FirstOrDefaultAsync(ev => ev.Id == id);

            if (eventToUpdate is not null) {
                eventToUpdate = updatedEvent;
                await _dbContext.SaveChangesAsync();
            }
            return null;
        }
    }
}
