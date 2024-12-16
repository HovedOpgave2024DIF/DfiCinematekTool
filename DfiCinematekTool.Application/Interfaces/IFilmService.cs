using DfiCinematekTool.Domain.Entities;

namespace DfiCinematekTool.Application.Interfaces
{
    public interface IFilmService
    {
        Task<List<Film>> GetAllFilmsAsync();
        Task<List<Film>> GetPaginatedFilmsAsync(int pageNumber, int pageSize);


    }
}
