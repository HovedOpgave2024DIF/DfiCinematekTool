using DfiCinematekTool.Domain.Entities;
using DfiCinematekTool.Domain.Interfaces;

namespace DfiCinematekTool.Application.Services
{
	public class UserService
	{
		private readonly IUserRepository _userRepository;

		public UserService(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<List<User>> GetAllUsersAsync()
		{
			try
			{
				return await _userRepository.GetAllUsersAsync();
			}
			catch (Exception ex)
			{
				//Implements logger for exception messages
				Console.WriteLine(ex.Message);
				throw;
			}
		}

		public async Task<User?> GetUserByUserNameAsync(string userName)
		{
			try
			{
				return await _userRepository.GetUserByUserNameAsync(userName);
			}
			catch (Exception ex)
			{
				//Implements logger for exception messages
				Console.WriteLine(ex.Message);
				throw;
			}
		}

		public async Task<User?> CreateUserAsync(User user)
		{
			try
			{
				return await _userRepository.CreateUserAsync(user);
			}
			catch (Exception ex)
			{
				//Implements logger for exception messages
				Console.WriteLine(ex.Message);
				throw;
			}
		}

		public async Task<User?> UpdateUserAsync(User user)
		{
			try
			{
				return await _userRepository.UpdateUserAsync(user);
			}
			catch (Exception ex)
			{
				//Implements logger for exception messages
				Console.WriteLine(ex.Message);
				throw;
			}
		}

		public async Task<bool> DeleteUserByUserNameAsync(string userName)
		{
			try
			{
				return await _userRepository.DeleteUserByUserNameAsync(userName);
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
