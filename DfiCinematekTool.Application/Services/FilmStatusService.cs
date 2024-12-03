using DfiCinematekTool.Domain.Entities;
using DfiCinematekTool.Domain.Interfaces;

namespace DfiCinematekTool.Application.Services
{
         
    public class FilmStatusService
    {

    private readonly IFilmStatusRepository _filmStatusRepository;
        public FilmStatusService(IFilmStatusRepository filmStatusRepository)
        {
            _filmStatusRepository = filmStatusRepository;
        }

        public async Task<FilmStatus> CreateFilmStatusAsync(FilmStatus newFilmStatus)
        {
            try
            {
                return await _filmStatusRepository.CreateFilmStatusAsync(newFilmStatus);
            }
            catch (Exception ex)
            {
                //Implements logger for exception messages
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<FilmStatus?> GetFilmStatusByIds(int eventId, int filmId)
        {
            try
            {
                return await _filmStatusRepository.GetFilmStatusByIds(eventId, filmId);
            }
            catch (Exception ex)
            {
                //Implements logger for exception messages
                Console.WriteLine(ex.Message);
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
                //Implements logger for exception messages
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteFilmStatusAsync(int eventId, int filmId)
        {
            try
            {
                return await _filmStatusRepository.DeleteFilmStatusAsync(int eventId, int filmId);
            }
            catch (Exception ex)
            {
                //Implements logger for exception messages
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
