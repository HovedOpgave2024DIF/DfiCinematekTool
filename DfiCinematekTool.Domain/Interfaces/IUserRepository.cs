using DfiCinematekTool.Domain.Entities;

namespace DfiCinematekTool.Domain.Interfaces
{
	public interface IUserRepository
	{
		Task<List<User>> GetAllUsersAsync();
		Task<User?> GetUserByUserNameAsync(string userName);
		Task<User?> CreateUserAsync(User user);
		Task<User?> UpdateUserAsync(User user);
		Task<bool> DeleteUserByUserNameAsync(string userName);
		Task<bool> LockoutUserAsync(string userName, bool isLocked);
	}
}
