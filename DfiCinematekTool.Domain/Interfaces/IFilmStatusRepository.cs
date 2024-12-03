using DfiCinematekTool.Domain.Entities;

namespace DfiCinematekTool.Domain.Interfaces
{
	public interface IFilmStatusRepository
	{
		Task<FilmStatus> CreateFilmStatusAsync(FilmStatus newFilmStatus);

		Task<FilmStatus?> GetFilmStatusByIdsAsync(int eventId, int filmId);

		Task<FilmStatus?> UpdateFilmStatusAsync(FilmStatus updateFilmStatus);

		Task<bool> DeleteFilmStatusAsync(int eventId, int filmId);
	}
}
