using DfiCinematekTool.Domain.Entities;
using DfiCinematekTool.Domain.Interfaces;
using DfiCinematekTool.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DfiCinematekTool.Infrastructure.Repositories
{
	public class ApplicationUserRepository : IUserRepository 
	{
		#region Fields & Contstructor
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationUserAuthorization _userAuthorization;

		public ApplicationUserRepository(UserManager<ApplicationUser> userManager, ApplicationUserAuthorization userAuthorization)
		{
			_userManager = userManager;
			_userAuthorization = userAuthorization;
		}
		#endregion

		#region Get all users
		public async Task<List<User>> GetAllUsersAsync()
		{
			var applicationUsers = await _userManager.Users.ToListAsync();

			var users = new List<User>();

			foreach (var applicationUser in applicationUsers)
			{
				if (applicationUser.UserName is null || applicationUser.Email is null) continue;
					
					// Find roles
					var roles = await _userManager.GetRolesAsync(applicationUser);

					users.Add(new User
					{
						Id = applicationUser.Id,
						UserName = applicationUser.UserName,
						Email = applicationUser.Email,
						Roles = roles.ToList(),
						Lockout = applicationUser.LockoutEnd
					});
			}
			return users;
		}
		#endregion

		#region Get user by name

		/// <summary>
		/// Finds a user by username.
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public async Task<User?> GetUserByUserNameAsync(string username)
		{
			if (string.IsNullOrWhiteSpace(username))
				throw new ArgumentNullException(nameof(username), "User name cannot be empty.");

			if (int.TryParse(username, out _))
				throw new ArgumentException($"User name '{username}' cannot be a number.", nameof(username));

			//Find the user
			var applicationUser = await _userManager.FindByNameAsync(username);
			var user = new User();

			if (applicationUser is null) return user;

			// Add roles
			if (applicationUser.UserName is null || applicationUser.Email is null) return user;
			var roles = await _userManager.GetRolesAsync(applicationUser);
			user.Id = applicationUser.Id;
			user.UserName = applicationUser.UserName;
			user.Email = applicationUser.Email;
			user.Roles = roles.ToList();

			return user;
		}
		#endregion

		#region Create user

		/// <summary>
		/// Creates an ApplicationUser by passing an User entity.
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="Exception"></exception>
		public async Task<User?> CreateUserAsync(User user)
		{
			if (user is null)
				throw new ArgumentNullException(nameof(user), "User cannot be empty.");

			var existingUserByName = await _userManager.FindByNameAsync(user.UserName);
			var existingUserByEmail = await _userManager.FindByEmailAsync(user.Email);

			if (existingUserByName != null || existingUserByEmail != null)
				throw new Exception($"User with username '{user.UserName}' or email '{user.Email}' already exists.");

			// Create new ApplicationUser
			var newApplicationUser = new ApplicationUser
			{
				UserName = user.UserName,
				Email = user.Email,
			};

			// Create user
			var result = await _userManager.CreateAsync(newApplicationUser, user.Password);

			if (!result.Succeeded)
				throw new Exception($"User '{user.UserName}' not created. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");

			// Add roles
			if (user.Roles is not null)
			{
				await _userAuthorization.HandleUserRolesAsync(user);
			}

			return user;
		}
		#endregion

		#region Update user

		/// <summary>
		/// Updates the ApplicationUser's information by recieving a User entity.
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="Exception"></exception>
		public async Task<User?> UpdateUserAsync(User user)
		{
			if (user is null)
				throw new ArgumentNullException(nameof(user), "User cannot be empty.");

			var applicationUser = await _userManager.FindByIdAsync(user.Id);

			if (applicationUser is null)
				throw new Exception($"User '{user.UserName}' not found.");

			// Update username
			if (!string.IsNullOrEmpty(user.UserName) && !user.UserName.Equals(applicationUser.UserName))
			{
				var existingUserByUsername = await _userManager.FindByNameAsync(user.UserName);
				if (existingUserByUsername != null && existingUserByUsername.Id != applicationUser.Id)
					throw new Exception($"Username '{user.UserName}' is already taken by another user.");

				applicationUser.UserName = user.UserName;
			}

			// Update email
			if (!string.IsNullOrEmpty(user.Email) && !user.Email.Equals(applicationUser.Email))
			{
				var existingUserByEmail = await _userManager.FindByEmailAsync(user.Email);
				if (existingUserByEmail != null && existingUserByEmail.Id != applicationUser.Id)
					throw new Exception($"Email '{user.Email}' is already taken by another user.");

				applicationUser.Email = user.Email;
			}

			// Update password
			if (!string.IsNullOrEmpty(user.Password))
			{
				var token = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);
				var passwordResult = await _userManager.ResetPasswordAsync(applicationUser, token, user.Password);

				if (!passwordResult.Succeeded)
					throw new Exception($"Failed to update password for user '{user.UserName}'. Errors: {string.Join(", ", passwordResult.Errors.Select(e => e.Description))}");
			}

			// Update roles
			if (user.Roles is not null)
			{
				await _userAuthorization.HandleUserRolesAsync(user);
			}
	
			// Update user in the database
			var updateResult = await _userManager.UpdateAsync(applicationUser);

			if (!updateResult.Succeeded)
				throw new Exception($"Failed to update user '{user.UserName}'. Errors: {string.Join(", ", updateResult.Errors.Select(e => e.Description))}");

			return user;
		}
		#endregion

		#region Delete user by name
		public async Task<bool> DeleteUserByUserNameAsync(string userName)
		{
			if (string.IsNullOrWhiteSpace(userName))
				throw new ArgumentNullException(nameof(userName), "User name cannot be empty.");

			if (int.TryParse(userName, out _))
				throw new ArgumentException($"User name '{userName}' cannot be a number.", nameof(userName));

			// Find user by username
			var applicationUserToRemove = await _userManager.FindByNameAsync(userName);

			if (applicationUserToRemove is null)
				return false; 

			// Delete user
			var userDeleted = await _userManager.DeleteAsync(applicationUserToRemove);

			if (!userDeleted.Succeeded)
				throw new Exception($"Failed to remove user '{applicationUserToRemove.UserName}'. Errors: {string.Join(", ", userDeleted.Errors.Select(e => e.Description))}");

			return true;
		}
		#endregion

		#region Lockout user
		public async Task<bool> LockoutUserAsync(string userName, bool isLocked)
		{
			return await _userAuthorization.HandleUserLockoutAsync(userName, isLocked);
		}
		#endregion
	}
}
