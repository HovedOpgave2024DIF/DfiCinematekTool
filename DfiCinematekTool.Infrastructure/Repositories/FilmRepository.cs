using DfiCinematekTool.Domain.Entities;
using DfiCinematekTool.Domain.Interfaces;
using DfiCinematekTool.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DfiCinematekTool.Infrastructure.Repositories
{
	public class FilmRepository : IFilmRepository
	{
		private readonly CinematekDbContext _dbContext;

		public FilmRepository(CinematekDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Film>> GetAllFilmsAsync()
		{
			return await _dbContext.Films.ToListAsync();
		}

		public async Task<List<Film>> GetPaginatedFilmsAsync(int pageNumber, int pageSize)
		{
			if (pageNumber < 1)
				throw new ArgumentOutOfRangeException(nameof(pageNumber), 
					"Page number must be greater than or equal to 1.");

			if (pageSize < 1)
				throw new ArgumentOutOfRangeException(nameof(pageSize), 
					"Page size must be greater than or equal to 1.");

			var skipNumber = (pageNumber - 1) * pageSize;

			return await _dbContext.Films.
				Skip(skipNumber).
				Take(pageSize).
				ToListAsync();
		}
	}
}
