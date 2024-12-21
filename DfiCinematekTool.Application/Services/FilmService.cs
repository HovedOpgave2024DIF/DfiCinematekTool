using DfiCinematekTool.Application.Interfaces;
using DfiCinematekTool.Domain.Entities;
using DfiCinematekTool.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace DfiCinematekTool.Application.Services
{
	public class FilmService : IFilmService
	{
		private readonly IFilmRepository _filmRepository;
		private readonly ILogger<FilmService> _logger;
		public FilmService(IFilmRepository filmRepository, ILogger<FilmService> logger)
		{
			_filmRepository = filmRepository;
			_logger = logger;
		}

		public async Task<List<Film>> GetAllFilmsAsync()
		{
			try
			{
				return await _filmRepository.GetAllFilmsAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error fetching all films");
				throw;
			}
		}

		public async Task<List<Film>> GetPaginatedFilmsAsync(int pageNumber, int pageSize)
		{
			try
			{
				return await _filmRepository.GetPaginatedFilmsAsync(pageNumber, pageSize);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error fetching paginated films");
				throw;
			}
		}
	}
}
