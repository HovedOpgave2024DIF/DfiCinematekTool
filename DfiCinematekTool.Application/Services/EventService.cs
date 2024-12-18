using DfiCinematekTool.Application.Interfaces;
using DfiCinematekTool.Domain.Entities;
using DfiCinematekTool.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace DfiCinematekTool.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<EventService> _logger; 
        public EventService(IEventRepository eventRepository, ILogger<EventService> logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
        }

		public async Task<List<Event>> GetAllEventsAsync()
		{
			try
			{
				return await _eventRepository.GetAllEventsAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error fetching all events");
				throw;
			}
		}

		public async Task<Event> CreateEventAsync(Event newEvent)
        {
            try
            {
                return await _eventRepository.CreateEventAsync(newEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating event: {Title}", newEvent.Title);
                throw;
            }
        }

        public async Task<List<Event>> GetPaginatedEventsAsync(int pageNumber, int pageSize)
        {
            try
            {
                return await _eventRepository.GetPaginatedEventsAsync(pageNumber,  pageSize);
            }
            catch (Exception ex)
            {
				_logger.LogError(ex, "Error fetcing paginated events");
				throw;
            }
        }

        public async Task<Event?> GetEventById(int id)
        {
            try
            {
                return await _eventRepository.GetEventByIdAsync(id);
            }
            catch (Exception ex)
            {
				_logger.LogError(ex, "Error fetching event with id: {id}", id);
				throw;
            }
        }

        public async Task<Event?> UpdateEvent(Event updatedEvent)
        {
            try
            {
                return await _eventRepository.UpdateEventAsync(updatedEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating event: {Title}", updatedEvent.Title);
                throw;
            }
        }

        public async Task<bool> DeleteEventById(int id)
        {
            try
            {
                return await _eventRepository.DeleteEventByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting event with id: {id}", id);
                throw;
            }
        }
    }
}
