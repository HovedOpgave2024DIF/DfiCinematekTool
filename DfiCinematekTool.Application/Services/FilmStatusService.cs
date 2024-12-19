using DfiCinematekTool.Application.Interfaces;
using DfiCinematekTool.Domain.Entities;
using DfiCinematekTool.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace DfiCinematekTool.Application.Services
{

	public class FilmStatusService : IFilmStatusService
	{
		private readonly IFilmStatusRepository _filmStatusRepository;
		private readonly ILogger<FilmStatusService> _logger;
		public FilmStatusService(IFilmStatusRepository filmStatusRepository, ILogger<FilmStatusService> logger)
		{
			_filmStatusRepository = filmStatusRepository;
			_logger = logger;
		}

		public async Task<FilmStatus> CreateFilmStatusAsync(FilmStatus newFilmStatus)
		{
			try
			{
				return await _filmStatusRepository.CreateFilmStatusAsync(newFilmStatus);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error creating film status");
				throw;
			}
		}

		public async Task<FilmStatus?> GetFilmStatusByIds(int eventId, int filmId)
		{
			try
			{
				return await _filmStatusRepository.GetFilmStatusByIdsAsync(eventId, filmId);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error fetching film status with event id: {evenId} and film id: {filmId}", eventId, filmId);
				throw;
			}
		}

		public async Task<FilmStatus?> UpdateFilmStatusAsync(FilmStatus updateFilmStatus)
		{
			try
			{
				return await _filmStatusRepository.UpdateFilmStatusAsync(updateFilmStatus);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating film status by id: {Id} ", updateFilmStatus.Id);
				throw;
			}
		}

		public async Task<bool> DeleteFilmStatusAsync(int eventId, int filmId)
		{
			try
			{
				return await _filmStatusRepository.DeleteFilmStatusAsync(eventId, filmId);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error fetching film status with event id: {evenId} and film id: {filmId}", eventId, filmId);
				throw;
			}
		}
	}
}
