using DfiCinematekTool.Domain.Entities;
using DfiCinematekTool.Domain.Interfaces;
using DfiCinematekTool.Infrastructure.Context;
using DfiCinematekTool.Infrastructure.Repositories;
using DfiCinematekTool.Infrastructure.Seeds;
using Microsoft.EntityFrameworkCore;

namespace DfiCinematekTool.Tests.Infrastructure
{
	public class EventRepositoryTests : IAsyncLifetime
	{
		private readonly CinematekDbContext _dbContext;
		private readonly EventRepository _eventRepository;

		public EventRepositoryTests()
		{
			var options = new DbContextOptionsBuilder<CinematekDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			_dbContext = new CinematekDbContext(options);

			var filmStatusRepository = new FilmStatusRepository(_dbContext);

			_eventRepository = new EventRepository(_dbContext, filmStatusRepository);
		}
		public async Task InitializeAsync()
		{
			await FilmsAndEvents.SeedData(_dbContext);
		}

		public async Task DisposeAsync()
		{
			await _dbContext.DisposeAsync();
		}

		[Fact]
		public async Task GetAllEventsAsync_ReturnsAllEvents()
		{
			// Act
			var events = await _eventRepository.GetAllEventsAsync();

			// Assert
			Assert.NotNull(events);
			Assert.Equal(2, events.Count);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		public async Task GetEventByIdAsync_ReturnsOneEvent(int eventId)
		{
			// Act
			var ev = await _eventRepository.GetEventById(eventId);

			// Assert
			Assert.NotNull(ev);
			Assert.Equal(eventId, ev.Id);
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(0)]
		public async Task GetEventById_ThrowsException_WhenIdIsInvalid(int eventId)
		{
			var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
				async () => await _eventRepository.GetEventById(eventId)
			);

			Assert.Equal("ID must be greater than 0. (Parameter 'id')", exception.Message);
		}

		[Theory]
		[InlineData(1, 1)]
		[InlineData(1, 2)]
		public async Task GetPaginatedEventsAsync_ReturnsCorrectEvents(int pageNumber, int pageSize)
		{
			// Act
			var events = await _eventRepository.GetPaginatedEventsAsync(pageNumber, pageSize);
			
			// Assert
			Assert.NotNull(events);
			Assert.True(events.Count <= pageSize);
		}

		[Theory]
		[InlineData(0, 2)]
		[InlineData(-1, 2)]
		public async Task GetPaginatedEventsAsync_ThrowsException_WhenPageNumberIsInvalid(int pageNumber, int pageSize)
		{
			// Act & Assert
			var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
				async () => await _eventRepository.GetPaginatedEventsAsync(pageNumber, pageSize)
			);

			Assert.Equal("Page number must be greater than or equal to 1. (Parameter 'pageNumber')", exception.Message);
		}

		[Theory]
		[InlineData(2, 0)]
		[InlineData(2, -1)]
		public async Task GetPaginatedEventsAsync_ThrowsException_WhenPageSizeIsInvalid(int pageNumber, int pageSize)
		{
			// Act & Assert
			var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
				async () => await _eventRepository.GetPaginatedEventsAsync(pageNumber, pageSize)
			);

			Assert.Equal("Page size must be greater than or equal to 1. (Parameter 'pageSize')", exception.Message);
		}

		// Create

		// Update
		[Fact]
		public async Task CreateEventAsync_AddsNewEventWithFilms()
		{
			// Arrange
			var newEvent = new Event
			{
				Title = "New Event",
				DateId = 123,
				Screen = "Screen A",
				DurationInMinutes = 120,
				Owner = "Test Owner",
				OwnerEmail = "owner@example.com",
				EventType = "Test Event Type",
				IsEvent = true,
				IsRooftop = false,
				Published = true,
				Abbriviation = "TEST",
				Films = new List<Film>
				{
					new Film { Id = 1 },
		            new Film { Id = 2 }
		        }
			};

			// Act
			var createdEvent = await _eventRepository.CreateEventAsync(newEvent);

			// Assert
			Assert.NotNull(createdEvent);
			Assert.Equal("New Event", createdEvent.Title);
			Assert.Equal(2, createdEvent?.Films?.Count);

			// Verify the event was saved in the database
			var savedEvent = await _eventRepository.GetEventById(createdEvent.Id);
			Assert.NotNull(savedEvent);
			Assert.Equal("New Event", savedEvent.Title);
			Assert.Equal(2, savedEvent?.Films?.Count);
		}

		[Fact]
		public async Task CreateEventAsync_ThrowsException_WhenFilmDoesNotExist()
		{
			// Arrange
			var newEvent = new Event
			{
				Title = "Invalid Event",
				DateId = 124,
				Screen = "Screen B",
				DurationInMinutes = 90,
				Owner = "Test Owner",
				OwnerEmail = "owner2@example.com",
				EventType = "Test Event Type 2",
				IsEvent = false,
				IsRooftop = false,
				Published = false,
				Abbriviation = "TEST2",
				Films = new List<Film>
				{
					new Film { Id = 99 }
		        }
			};

			// Act & Assert
			var exception = await Assert.ThrowsAsync<InvalidOperationException>(
				async () => await _eventRepository.CreateEventAsync(newEvent)
			);

			Assert.Equal("Films with 99 does not exist.", exception.Message);
		}

		[Fact]
		public async Task DeleteEventById_ReturnsTrueIfEventHasBeenDeleted()
		{
			// Arrange
			const int eventId = 1;

			// Act
			var hasBeenDeleted = await _eventRepository.DeleteEventById(eventId);

			// Assert
			Assert.True(hasBeenDeleted);

			// Verify the event has been deleted
			var eventDeleted = await _eventRepository.GetEventById(eventId);
			Assert.Null(eventDeleted);
		}

		[Fact]
		public async Task DeleteEventById_ReturnsFalseIfEventDoesNotExist()
		{
			// Arrange
			const int eventId = 3;

			// Act
			var hasNotBeenDeleted = await _eventRepository.DeleteEventById(eventId);

			// Assert
			Assert.False(hasNotBeenDeleted);
		}

		[Fact]
		public async Task DeleteEventById_ThrowsException_WhenIdIsInvalid()
		{
			// Arrange
			const int eventId = 0;

			// Act & Assert
			var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
				async () => await _eventRepository.DeleteEventById(eventId)
			);

			Assert.Equal("ID must be greater than 0. (Parameter 'id')", exception.Message);
		}

		// Test UpdateEvents
	}
}
