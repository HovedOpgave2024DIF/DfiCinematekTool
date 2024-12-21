using DfiCinematekTool.Application.Interfaces;
using DfiCinematekTool.Domain.Entities;
using DfiCinematekTool.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace DfiCinematekTool.Application.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly ILogger<UserService> _logger;

		public UserService(IUserRepository userRepository, ILogger<UserService> logger)
		{
			_userRepository = userRepository;
			_logger = logger;
		}

		public async Task<List<User>> GetAllUsersAsync()
		{
			try
			{
				return await _userRepository.GetAllUsersAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error fetching all users.");
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
				_logger.LogError(ex, "Error fetching user by username: {Username}", userName);
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
				_logger.LogError(ex, "Error creating user: {UserName}", user.UserName);
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
				_logger.LogError(ex, "Error updating user: {UserName}", user.UserName);
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
				_logger.LogError(ex, "Error deleting user: {Username}", userName);
				throw;
			}
		}

		public async Task<bool> HandleUserLockoutAsync(string userName, bool isLocked)
		{
			try
			{
				return await _userRepository.LockoutUserAsync(userName, isLocked);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error handling user lockout for: {Username}", userName);
				throw;
			}
		}
	}
}
