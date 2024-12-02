using DfiCinematekTool.Domain.Entities;
using DfiCinematekTool.Infrastructure.Context;
using DfiCinematekTool.Infrastructure.Repositories;
using DfiCinematekTool.Infrastructure.Seeds;
using Microsoft.EntityFrameworkCore;

namespace DfiCinematekTool.Tests.Infrastructure
{
	public class FilmRepositoryTests : IAsyncLifetime
	{
		private readonly CinematekDbContext _dbContext;
		private readonly FilmRepository _filmRepository;

		public FilmRepositoryTests()
		{
			var options = new DbContextOptionsBuilder<CinematekDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			_dbContext = new CinematekDbContext(options);
			_filmRepository = new FilmRepository(_dbContext);
		}
		public async Task InitializeAsync()
		{
			await FilmsAndEvents.SeedData(_dbContext);
		}

		public async Task DisposeAsync()
		{
			await _dbContext.DisposeAsync();
		}

		//private static async Task SeedDatabase(CinematekDbContext dbContext)
		//{
		//	var films = new List<Film>
		//	{
		//		new Film { Title = "Den lille fugl og larven", Format = "DCP", DurationInMinutes = 4, SortOrder = 1 },
		//		new Film { Title = "Forårsfest", Format = "DCP", DurationInMinutes = 5, SortOrder = 2 },
		//		new Film { Title = "Astons gaver", OriginalTitle = "Astons presenter", Format = "DCP", DurationInMinutes = 9, SortOrder = 3 },
		//		new Film { Title = "En lille smule", OriginalTitle = "Bara Lite", Format = "DCP", DurationInMinutes = 9, SortOrder = 4 },
		//		new Film { Title = "Min oldefars historier - Ridderne", Format = "DCP", DurationInMinutes = 9, SortOrder = 5 },
		//		new Film { Title = "Pipfugle", Format = "DCP", DurationInMinutes = 9, SortOrder = 6 },
		//	};

		//	await dbContext.Films.AddRangeAsync(films);
		//	await dbContext.SaveChangesAsync();
		//}

		[Fact]
		public async Task GetAllFilmsAsync_ReturnsAllFilms()
		{
			// Act
			var films = await _filmRepository.GetAllFilmsAsync();

			// Assert
			Assert.NotNull(films);
			Assert.Equal(6, films.Count);
		}

		[Theory]
		[InlineData(1, 2)]
		[InlineData(2, 3)]
		[InlineData(3, 10)]
		public async Task GetPaginatedFilmsAsync_ReturnsCorrectFilms(int pageNumber, int pageSize)
		{
			// Act
			var films = await _filmRepository.GetPaginatedFilmsAsync(pageNumber, pageSize);

			// Assert
			Assert.NotNull(films);
			Assert.True(films.Count <= pageSize);
		}

		[Theory]
		[InlineData(0, 10)]
		[InlineData(-1, 10)]

		public async Task GetPaginatedFilmsAsync_ThrowsException_WhenPageNumberIsInvalid(int pageNumber, int pageSize)
		{
			// Act & Assert
			var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
				async () => await _filmRepository.GetPaginatedFilmsAsync(pageNumber, pageSize)
			);

			Assert.Equal("Page number must be greater than or equal to 1. (Parameter 'pageNumber')", exception.Message);
		}

		[Theory]
		[InlineData(10, 0)]
		[InlineData(10, -1)]
		public async Task GetPaginatedFilmsAsync_ThrowsException_WhenPageSizeIsInvalid(int pageNumber, int pageSize)
		{
			// Act & Assert
			var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
				async () => await _filmRepository.GetPaginatedFilmsAsync(pageNumber, pageSize)
			);

			Assert.Equal("Page size must be greater than or equal to 1. (Parameter 'pageSize')", exception.Message);
		}
	}
}
