using DfiCinematekTool.Domain.Entities;
using DfiCinematekTool.Domain.Interfaces;
using DfiCinematekTool.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DfiCinematekTool.Infrastructure.Repositories
{
	public class FilmStatusRepository : IFilmStatusRepository
	{
		#region Fields & Constructor
		private readonly CinematekDbContext _dbContext;

		public FilmStatusRepository(CinematekDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		#endregion

		#region Get film status by id
		public async Task<FilmStatus?> GetFilmStatusByIdsAsync(int eventId, int filmId)
		{
			if (eventId < 1)
				throw new ArgumentOutOfRangeException(nameof(eventId), "Event id cannot be 0.");

			if (filmId < 1)
				throw new ArgumentOutOfRangeException(nameof(eventId), "Film id cannot be 0.");

			return await _dbContext.FilmStatuses.
				FirstOrDefaultAsync(fs => fs.EventId == eventId && fs.FilmId == filmId);
		}
		#endregion

		#region Create film status

		/// <summary>
		/// Creates a films status given that the FilmStatus entity has an event id and film id
		/// </summary>
		/// <param name="newFilmStatus"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public async Task<FilmStatus> CreateFilmStatusAsync(FilmStatus newFilmStatus)
		{
			if (newFilmStatus is null)
				throw new ArgumentNullException(nameof(newFilmStatus), "New film status cannot be null.");

			var existingFilmStatus = await _dbContext.FilmStatuses
				.FirstOrDefaultAsync(fs => fs.EventId == newFilmStatus.EventId && fs.FilmId == newFilmStatus.FilmId);

			if (existingFilmStatus is not null)
				throw new InvalidOperationException($"FilmStatus with EventId {newFilmStatus.EventId} and FilmId {newFilmStatus.FilmId} already exists.");


			await _dbContext.FilmStatuses.AddAsync(newFilmStatus);
			await _dbContext.SaveChangesAsync();

			return newFilmStatus;
		}
		#endregion

		#region Update film status

		public async Task<FilmStatus?> UpdateFilmStatusAsync(FilmStatus updateFilmStatus)
		{
			if (updateFilmStatus is null)
				throw new ArgumentNullException(nameof(updateFilmStatus), "Updated film status cannot be null.");

			var filmStatusToUpdate = await _dbContext.FilmStatuses.FindAsync(updateFilmStatus.Id);

			if (filmStatusToUpdate is null)
				throw new Exception($"Film status not found.");

			await _dbContext.SaveChangesAsync();

			return filmStatusToUpdate;
		}
		#endregion

		#region Delete film status
		public async Task<bool> DeleteFilmStatusAsync(int eventId, int filmId)
		{
			if (eventId <= 0)
				throw new ArgumentOutOfRangeException(nameof(eventId), "Event id cannot be 0.");

			if (filmId <= 0)
				throw new ArgumentOutOfRangeException(nameof(filmId), "Film id cannot be 0.");

			var filmStatus = await GetFilmStatusByIdsAsync(eventId, filmId);

			if (filmStatus is null)
				return false;

			_dbContext.FilmStatuses.Remove(filmStatus);
			await _dbContext.SaveChangesAsync();

			return true;
		}
		#endregion
	}
}
