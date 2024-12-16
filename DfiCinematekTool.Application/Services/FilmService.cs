using DfiCinematekTool.Application.Interfaces;
using DfiCinematekTool.Domain.Entities;
using DfiCinematekTool.Domain.Interfaces;

namespace DfiCinematekTool.Application.Services
{
    public class FilmService : IFilmService
    {
        private readonly IFilmRepository _filmRepository;
        public FilmService(IFilmRepository filmRepository)
        {
            _filmRepository = filmRepository;
        }

        public async Task<List<Film>> GetAllFilmsAsync()
        {
            try
            {
                return await _filmRepository.GetAllFilmsAsync();
            }
            catch (Exception ex)
            {
                //Implements logger for exception messages
                Console.WriteLine(ex.Message);
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
                //Implements logger for exception messages
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
