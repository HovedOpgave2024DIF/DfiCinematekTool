using DfiCinematekTool.Domain.Entities;
using DfiCinematekTool.Domain.Interfaces;
using DfiCinematekTool.Infrastructure.Context;
using DfiCinematekTool.Infrastructure.Identity;
using DfiCinematekTool.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DfiCinematekTool.Infrastructure.Repositories
{
	public class ApplicationUserRepository : IUserRepository 
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly CinematekDbContext _dbContext;

		public ApplicationUserRepository(UserManager<ApplicationUser> userManager, CinematekDbContext dbContext)
		{
			_userManager = userManager;
			_dbContext = dbContext;
		}

		public async Task<List<User>> GetAllUsersAsync()
		{
			var applicationUsers = await _userManager.Users.ToListAsync();

			var users = new List<User>();

			foreach (var applicationUser in applicationUsers)
			{
				if (applicationUser.UserName is null || applicationUser.Email is null) continue;

					var roles = await _userManager.GetRolesAsync(applicationUser);

					users.Add(new User
					{
						UserName = applicationUser.UserName,
						Email = applicationUser.Email,
						Roles = roles.ToList()
					});
			}
			return users;
		}


		public async Task<User?> GetUserByUserNameAsync(string userName)
		{
			if (string.IsNullOrWhiteSpace(userName))
				throw new ArgumentNullException(nameof(userName), "User name cannot be empty.");

			if (int.TryParse(userName, out _))
				throw new ArgumentException($"User name '{userName}' cannot be a number.", nameof(userName));

			//Find the user by username
			var applicationUser = await _userManager.FindByEmailAsync(userName);
			var user = new User();

			if (applicationUser is null) return user;

			// Add roles
			if (applicationUser.UserName is null || applicationUser.Email is null) return user;
			var roles = await _userManager.GetRolesAsync(applicationUser);
			user.UserName = applicationUser.UserName;
			user.Email = applicationUser.Email;
			user.Roles = roles.ToList();

			return user;
		}

		public async Task<User?> CreateUserAsync(User user)
		{
			if (user is null)
				throw new ArgumentNullException(nameof(user), "User cannot be empty.");

			var existingUserByName = await _userManager.FindByNameAsync(user.UserName);
			var existingUserByEmail = await _userManager.FindByEmailAsync(user.Email);

			if (existingUserByName != null || existingUserByEmail != null)
				throw new Exception($"User with username '{user.UserName}' or email '{user.Email}' already exists.");

			// Create new user
			var newApplicationUser = new ApplicationUser
			{
				UserName = user.UserName,
				Email = user.Email,
			};

			var result = await _userManager.CreateAsync(newApplicationUser, user.Password);

			if (!result.Succeeded)
				throw new Exception($"User '{user.UserName}' not created. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");

			// Add roles
			if (user.Roles is not null)
			{
				var roleResult = await _userManager.AddToRolesAsync(newApplicationUser, user.Roles);

				if (!roleResult.Succeeded)
					throw new Exception($"Failed to assign roles to user '{user.UserName}'. Errors: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
			}
			return user;
		}

		public async Task<User?> UpdateUserAsync(User user)
		{
			if (user is null)
				throw new ArgumentNullException(nameof(user), "User cannot be empty.");

			var applicationUser = await _userManager.FindByNameAsync(user.UserName);

			if (applicationUser is null)
				throw new Exception($"User '{user.UserName}' not found.");

			// Update email
			if (!string.IsNullOrEmpty(applicationUser.Email) && applicationUser.Email.Equals(user.Email))
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
				var currentRoles = await _userManager.GetRolesAsync(applicationUser);

				var rolesToAdd = user.Roles.Except(currentRoles).ToList();
				var rolesToRemove = currentRoles.Except(user.Roles).ToList();

				if (rolesToAdd.Count > 0)
				{
					var addRolesResult = await _userManager.AddToRolesAsync(applicationUser, rolesToAdd);

					if (!addRolesResult.Succeeded)
						throw new Exception($"Failed to add roles to user '{user.UserName}'. Errors: {string.Join(", ", addRolesResult.Errors.Select(e => e.Description))}");
				}

				if (rolesToRemove.Count > 0)
				{
					var removeRolesResult = await _userManager.RemoveFromRolesAsync(applicationUser, rolesToRemove);

					if (!removeRolesResult.Succeeded)
						throw new Exception($"Failed to remove roles from user '{user.UserName}'. Errors: {string.Join(", ", removeRolesResult.Errors.Select(e => e.Description))}");
				}
			}

			// Update user in the database
			var updateResult = await _userManager.UpdateAsync(applicationUser);

			if (!updateResult.Succeeded)
				throw new Exception($"Failed to update user '{user.UserName}'. Errors: {string.Join(", ", updateResult.Errors.Select(e => e.Description))}");

			return user;
		}

		public async Task<bool> DeleteUserByUserNameAsync(string userName)
		{
			if (string.IsNullOrWhiteSpace(userName))
				throw new ArgumentNullException(nameof(userName), "User name cannot be empty.");

			if (int.TryParse(userName, out _))
				throw new ArgumentException($"User name '{userName}' cannot be a number.", nameof(userName));

			// Find the user by username
			var applicationUserToRemove = await _userManager.FindByNameAsync(userName);

			if (applicationUserToRemove is null)
				return false; 

			// Delete the user
			var userDeleted = await _userManager.DeleteAsync(applicationUserToRemove);

			if (!userDeleted.Succeeded)
				throw new Exception($"Failed to remove user '{applicationUserToRemove.UserName}'. Errors: {string.Join(", ", userDeleted.Errors.Select(e => e.Description))}");

			return true;
		}
	}
}
