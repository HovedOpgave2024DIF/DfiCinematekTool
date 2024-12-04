using DfiCinematekTool.Domain.Entities;

namespace DfiCinematekTool.Domain.Interfaces
{
    public interface IEventRepository
    {
	    Task<Event> CreateEventAsync(Event newEvent);

	    Task<List<Event>> GetAllEventsAsync();

	    Task<Event?> GetEventByIdAsync(int id);

	    Task<Event?> UpdateEventAsync(Event updateEvent);

	    Task<bool> DeleteEventByIdAsync(int id);

	    Task<List<Event>> GetPaginatedEventsAsync(int pageNumber, int pageSize);
	}
}
