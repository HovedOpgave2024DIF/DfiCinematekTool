using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DfiCinematekTool.Domain.Entities;
using DfiCinematekTool.Domain.Interfaces;

namespace DfiCinematekTool.Application.Services
{
    public class EventService
    {
        private readonly IEventRepository _eventRepository;
        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<Event> CreateEventAsync(Event newEvent)
        {
            try
            {
                return await _eventRepository.CreateEventAsync(newEvent);
            }
            catch (Exception ex)
            {
                //Implements logger for exception messages
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<List<Event>> GetAllEventsAsync()
        {
            try
            {
                return await _eventRepository.GetAllEventsAsync();
            }
            catch (Exception ex)
            {
                //Implements logger for exception messages
                Console.WriteLine(ex.Message);
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
                //Implements logger for exception messages
                Console.WriteLine(ex.Message);
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
                //Implements logger for exception messages
                Console.WriteLine(ex.Message);
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
                //Implements logger for exception messages
                Console.WriteLine(ex.Message);
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
                //Implements logger for exception messages
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
