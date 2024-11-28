
using DfiCinematekTool.Domain.Entities;

namespace DfiCinematekTool.Domain.Interfaces
{
    public interface IEventRepository
    {

        Task<Event> CreateEventAsync(Event newEvent);
        Task<List<Event>> GetAllEventsAsync();

        Task<Event?> GetEventByIdAsync(int id);

        Task<Event?> DeleteEventAsync(int id);

        Task<Event?> UpdateEventAsync(int id, Event updatedEvent);


    }
}
