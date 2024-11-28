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
	}
}
