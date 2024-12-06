using DfiCinematekTool.Domain.Entities;

namespace DfiCinematekTool.Application.Interfaces
{
	public interface IUserService
	{
		Task<List<User>> GetAllUsersAsync();
		Task<User?> GetUserByUserNameAsync(string userName);
		Task<User?> CreateUserAsync(User user);
		Task<User?> UpdateUserAsync(User user);
		Task<bool> DeleteUserByUserNameAsync(string userName);
	}
}
