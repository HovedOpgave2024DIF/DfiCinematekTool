using DfiCinematekTool.Domain.Entities;

namespace DfiCinematekTool.Application.Interfaces
{
	public interface IFilmStatusService
	{
		Task<FilmStatus> CreateFilmStatusAsync(FilmStatus newFilmStatus);

		Task<FilmStatus?> GetFilmStatusByIds(int eventId, int filmId);

		Task<FilmStatus?> UpdateFilmStatusAsync(FilmStatus updateFilmStatus);

		Task<bool> DeleteFilmStatusAsync(int eventId, int filmId);
	}
}
