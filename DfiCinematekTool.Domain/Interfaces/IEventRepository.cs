using DfiCinematekTool.Domain.Entities;

namespace DfiCinematekTool.Domain.Interfaces
{
    public interface IEventRepository
    {
	    Task<Event> CreateEventAsync(Event newEvent);

	    Task<List<Event>> GetAllEventsAsync();

	    Task<Event?> GetEventById(int id);

	    Task<Event?> UpdateEvent(Event updateEvent);

	    Task<bool> DeleteEventById(int id);

	    Task<List<Event>> GetPaginatedEventsAsync(int pageNumber, int pageSize);
	}
}
