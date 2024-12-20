using DfiCinematekTool.Domain.Entities;
using DfiCinematekTool.Infrastructure.Context;
using DfiCinematekTool.Infrastructure.Repositories;
using DfiCinematekTool.Infrastructure.Seeds;
using Microsoft.EntityFrameworkCore;

namespace DfiCinematekTool.Tests.Infrastructure
{
	public class FilmStatusRepositoryTests : IAsyncLifetime
	{
		private readonly CinematekDbContext _dbContext;
		private readonly FilmStatusRepository _filmStatusRepository;

		public FilmStatusRepositoryTests()
		{
			var options = new DbContextOptionsBuilder<CinematekDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			_dbContext = new CinematekDbContext(options);
			_filmStatusRepository = new FilmStatusRepository(_dbContext);
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
		public async Task CreateFilmStatus_AddsNewFilmStatus()
		{
			// Arrange

			const int eventId = 1;
			const int filmId = 3;

			var newFilmStatus = new FilmStatus
			{
				EventId = 1,
				FilmId = 3,
				ReceivedDate = DateTime.Now,
				CheckedDate = DateTime.Now,
				PreparedDate = DateTime.Now,
				Comment = "Test Comment",
				HasKey = true
			};

			// Act
			var createdFilmStatus = await _filmStatusRepository.CreateFilmStatusAsync(newFilmStatus);

			// Assert
			Assert.NotNull(createdFilmStatus);
			Assert.Equal(newFilmStatus.EventId, createdFilmStatus.EventId);
			Assert.Equal(newFilmStatus.FilmId, createdFilmStatus.FilmId);
			Assert.Equal(newFilmStatus.Comment, createdFilmStatus.Comment);

			var dbFilmStatus = await _filmStatusRepository.GetFilmStatusByIdsAsync(eventId, filmId);
			Assert.NotNull(dbFilmStatus);
		}


		[Fact]
		public async Task CreateFilmStatus_ThrowsException_IfNewFilmStatusIsNull()
		{
			// Act & Assert
			var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
				await _filmStatusRepository.CreateFilmStatusAsync(null));

			Assert.Equal("New film status cannot be null. (Parameter 'newFilmStatus')", exception.Message);
		}


		[Fact]
		public async Task CreateFilmStatus_ThrowsException_IfFilmStatusExists()
		{
			// Arrange
			var duplicateFilmStatus = new FilmStatus
			{
				EventId = 2,
				FilmId = 1,
				ReceivedDate = DateTime.Now,
				CheckedDate = DateTime.Now,
				PreparedDate = DateTime.Now,
				Comment = "Duplicate Test",
				HasKey = false
			};

			// Act & Assert
			var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
				await _filmStatusRepository.CreateFilmStatusAsync(duplicateFilmStatus));

			Assert.Equal("FilmStatus with EventId 2 and FilmId 1 already exists.", exception.Message);
		}

		[Theory]
		[InlineData(2,1)]
		[InlineData(2,2)]
		public async Task GetFilmStatusById_ReturnsFilmStatus(int eventId, int filmId)
		{
			// Act
			var filmStatus = await _filmStatusRepository.GetFilmStatusByIdsAsync(eventId, filmId);

			// Assert
			Assert.NotNull(filmStatus);
			Assert.Equal(eventId, filmStatus.EventId);
			Assert.Equal(filmId, filmStatus.FilmId);
		}


		[Fact]
		public async Task GetFilmStatusById_ThrowsException_WhenEventIdIsInvalid()
		{
			// Act & Assert
			var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
				await _filmStatusRepository.GetFilmStatusByIdsAsync(0, 1));

			Assert.Equal("Event id cannot be 0. (Parameter 'eventId')", exception.Message);
		}


		[Fact]
		public async Task GetFilmStatusById_ThrowsException_WhenFilmIdIsInvalid()
		{
			// Act & Assert
			var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
				await _filmStatusRepository.GetFilmStatusByIdsAsync(1, 0));

			Assert.Equal("Film id cannot be 0. (Parameter 'eventId')", exception.Message);
		}


		[Fact]
		public async Task UpdateFilmStatus_UpdatesExistingFilmStatus()
		{
			// Arrange

			const int eventId = 2;
			const int filmId = 1;

			var updatedFilmStatus = new FilmStatus
			{
				Id = 1,
				EventId = 2,
				FilmId = 1,
				ReceivedDate = DateTime.Now.AddDays(1),
				CheckedDate = DateTime.Now.AddDays(1),
				PreparedDate = DateTime.Now.AddDays(1),
				Comment = "Updated Comment",
				HasKey = false
			};

			// Act
			var result = await _filmStatusRepository.UpdateFilmStatusAsync(updatedFilmStatus);

			// Assert
			Assert.NotNull(result);
			Assert.Equal("Updated Comment", result.Comment);
			Assert.False(result.HasKey);

			var dbFilmStatus = await _filmStatusRepository.GetFilmStatusByIdsAsync(eventId, filmId);
			Assert.NotNull(dbFilmStatus);
			Assert.Equal("Updated Comment", dbFilmStatus.Comment);
		}


		[Fact]
		public async Task UpdateFilmStatus_ThrowsException_IfUpdateFilmStatusIsNull()
		{
			// Act & Assert
			var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
				await _filmStatusRepository.UpdateFilmStatusAsync(null));

			Assert.Equal("Updated film status cannot be null. (Parameter 'updateFilmStatus')", exception.Message);
		}


		[Fact]
		public async Task UpdateFilmStatus_ReturnNullIfFilmStatusDoNotExist()
		{
			// Arrange
			var nonExistentFilmStatus = new FilmStatus
			{
				Id = 999,
				EventId = 1,
				FilmId = 99,
				Comment = "Non-existent FilmStatus"
			};

			// Act
			var result = await _filmStatusRepository.UpdateFilmStatusAsync(nonExistentFilmStatus);

			// Assert
			Assert.Null(result);
		}


		[Theory]
		[InlineData(2, 1)]
		[InlineData(2,2)]
		public async Task DeleteFilmStatus_ReturnTrueWhenDeleted(int eventId, int filmId)
		{
			// Act
			var filmStatusIsDeleted = await _filmStatusRepository
				.DeleteFilmStatusAsync(eventId, filmId);

            // Assert
            Assert.True(filmStatusIsDeleted);

            var dbFilmStatus = await _filmStatusRepository
				.GetFilmStatusByIdsAsync(eventId, filmId);

			Assert.Null(dbFilmStatus);
		}

		[Theory]
		[InlineData(7, 1)]
		[InlineData(8, 2)]
		public async Task DeleteFilmStatus_ReturnFalseWhenNotDeleted(int eventId, int filmId)
		{
			// Act
			var filmStatusIsNotDeleted = await _filmStatusRepository
				.DeleteFilmStatusAsync(eventId, filmId);

            // Assert
            Assert.False(filmStatusIsNotDeleted);

            var filmStatsNotDeleted = await _filmStatusRepository
				.GetFilmStatusByIdsAsync(eventId, filmId);
			
			Assert.Null(filmStatsNotDeleted);
		}

		[Fact]
		public async Task DeleteFilmStatus_ThrowsException_WhenEventIdIsNotValid()
		{
			// Arrange
			const int eventId = 0;
			const int filmId = 1;

			// Act & Assert
			var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
				async () => await _filmStatusRepository.DeleteFilmStatusAsync(eventId, filmId)
			);

			Assert.Equal("Event id cannot be 0. (Parameter 'eventId')", exception.Message);
		}

		[Fact]
		public async Task DeleteFilmStatus_ThrowsException_WhenFilmIdIsNotValid()
		{
			// Arrange
			const int eventId = 2;
			const int filmId = 0;

			// Act & Assert
			var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
				async () => await _filmStatusRepository.DeleteFilmStatusAsync(eventId, filmId)
			);

			Assert.Equal("Film id cannot be 0. (Parameter 'filmId')", exception.Message);
		}
	}
}
