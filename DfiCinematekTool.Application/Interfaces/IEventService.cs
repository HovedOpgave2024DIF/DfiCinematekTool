using DfiCinematekTool.Domain.Entities;

namespace DfiCinematekTool.Application.Interfaces
{
	public interface IEventService
	{
		Task<Event> CreateEventAsync(Event newEvent);

		Task<List<Event>> GetAllEventsAsync();

		Task<List<Event>> GetPaginatedEventsAsync(int pageNumber, int pageSize);

		Task<Event?> GetEventById(int id);

		Task<Event?> UpdateEvent(Event updatedEvent);

		Task<bool> DeleteEventById(int id);
	}
}
