
using DfiCinematekTool.Domain.Entities;

namespace DfiCinematekTool.Domain.Interfaces
{
	public interface IFilmRepository
	{
		Task<List<Film>> GetAllFilmsAsync();
	}
}
