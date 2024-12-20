using DfiCinematekTool.Domain.Entities;
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
			var ev = await _eventRepository.GetEventByIdAsync(eventId);

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
				async () => await _eventRepository.GetEventByIdAsync(eventId)
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

			var savedEvent = await _eventRepository.GetEventByIdAsync(createdEvent!.Id);
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
			var hasBeenDeleted = await _eventRepository.DeleteEventByIdAsync(eventId);

			// Assert
			Assert.True(hasBeenDeleted);

			var eventDeleted = await _eventRepository.GetEventByIdAsync(eventId);
			Assert.Null(eventDeleted);
		}

		[Fact]
		public async Task DeleteEventById_ReturnsFalseIfEventDoesNotExist()
		{
			// Arrange
			const int eventId = 3;

			// Act
			var hasNotBeenDeleted = await _eventRepository.DeleteEventByIdAsync(eventId);

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
				async () => await _eventRepository.DeleteEventByIdAsync(eventId)
			);

			Assert.Equal("ID must be greater than 0. (Parameter 'id')", exception.Message);
		}

		[Fact]
		public async Task UpdateEvent_UpdatesEventDetailsSuccessfully()
		{
			// Arrange
			var updatedEvent = new Event
			{
				Id = 1,
				Title = "Updated Event Title",
				DateId = 456,
				Screen = "Updated Screen",
				DurationInMinutes = 150,
				Owner = "Updated Owner",
				OwnerEmail = "updatedowner@example.com",
				EventType = "Updated Event Type",
				IsEvent = false,
				IsRooftop = true,
				Published = true,
				Abbriviation = "UPD",
				Films = new List<Film>
				{
					new Film { Id = 2 },
					new Film { Id = 3 },
					new Film { Id = 1 }
				}
			};

			// Act
			var updatedResult = await _eventRepository.UpdateEventAsync(updatedEvent);

			// Assert
			Assert.NotNull(updatedResult);
			Assert.Equal("Updated Event Title", updatedResult.Title);
			Assert.Equal(150, updatedResult.DurationInMinutes);
			Assert.Equal(3, updatedResult?.Films?.Count);

			var dbEvent = await _eventRepository.GetEventByIdAsync(1);
			Assert.NotNull(dbEvent);
			Assert.Equal("Updated Event Title", dbEvent.Title);
			Assert.Equal(3, dbEvent.Films?.Count);
		}

		[Fact]
		public async Task UpdateEvent_AddsNewFilmsToEvent()
		{
			// Arrange
			var updatedEvent = new Event
			{
				Id = 1,
				Title = "Event with Added Films",
				DateId = 123,
				Films = new List<Film>
				{
					new Film { Id = 1 },
					new Film { Id = 2 },
					new Film { Id = 3 } // Adding a new film
		        }
			};

			// Act
			var updatedResult = await _eventRepository.UpdateEventAsync(updatedEvent);

			// Assert
			Assert.NotNull(updatedResult);
			Assert.Contains(updatedResult.Films, f => f.Id == 3);

			var dbEvent = await _eventRepository.GetEventByIdAsync(1);
			Assert.NotNull(dbEvent);
			Assert.Contains(dbEvent.Films, f => f.Id == 3);
		}

		[Fact]
		public async Task UpdateEvent_RemovesFilmsFromEvent()
		{
			// Arrange
			var updatedEvent = new Event
			{
				Id = 1,
				Title = "Event with Removed Films",
				DateId = 123,
				Films = new List<Film>
				{
					new Film { Id = 2 }
		        }
			};

			// Act
			var updatedResult = await _eventRepository.UpdateEventAsync(updatedEvent);

			// Assert
			Assert.NotNull(updatedResult);
			Assert.Single(updatedResult.Films!);
			Assert.Contains(updatedResult.Films!, f => f.Id == 2);

			var dbEvent = await _eventRepository.GetEventByIdAsync(1);
			Assert.NotNull(dbEvent);
			Assert.Single(dbEvent.Films!);
			Assert.Contains(dbEvent.Films!, f => f.Id == 2);
		}

		[Fact]
		public async Task UpdateEvent_ThrowsException_WhenEventIsNull()
		{
			// Act & Assert
			var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
			{
				await _eventRepository.UpdateEventAsync(null);
			});

			Assert.Equal("Updated event cannot be null. (Parameter 'updatedEvent')", exception.Message);
		}

		[Fact]
		public async Task UpdateEvent_ReturnsNull_WhenEventDoesNotExist()
		{
			// Arrange
			var updatedEvent = new Event
			{
				Id = 99,
				Title = "Non-existent Event"
			};

			// Act
			var result = await _eventRepository.UpdateEventAsync(updatedEvent);

			// Assert
			Assert.Null(result);
		}

	}
}
