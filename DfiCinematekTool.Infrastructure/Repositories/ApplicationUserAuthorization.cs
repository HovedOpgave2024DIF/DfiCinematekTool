using DfiCinematekTool.Infrastructure.Identity;
using DfiCinematekTool.Infrastructure.Interfaces;
using DfiCinematekTool.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using DfiCinematekTool.Domain.Entities;

namespace DfiCinematekTool.Infrastructure.Repositories
{
	public class ApplicationUserAuthorization : IAuthorization
	{
		#region Fields & Contructor
		private readonly UserManager<ApplicationUser> _userManager;
		public ApplicationUserAuthorization(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}
		#endregion

		#region Handle user lockout

		/// <summary>
		/// Enables and disables a user to login to the application.
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="isLocked"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="KeyNotFoundException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public async Task<bool> HandleUserLockoutAsync(string userName, bool isLocked)
		{
			if (string.IsNullOrWhiteSpace(userName))
				throw new ArgumentNullException(nameof(userName), "User name cannot be empty.");

			var applicationUser = await _userManager.FindByNameAsync(userName);

			if (applicationUser is null)
				throw new KeyNotFoundException($"User '{userName}' not found.");

			if (isLocked)
			{
				// Lock user
				var lockoutResult = await _userManager.SetLockoutEndDateAsync(applicationUser, DateTimeOffset.MaxValue);

				if (!lockoutResult.Succeeded)
					throw new InvalidOperationException($"Failed to lock out user '{userName}'. Errors: {string.Join(", ", lockoutResult.Errors.Select(e => e.Description))}");
			}
			else
			{
				// Unlock user
				var unlockResult = await _userManager.SetLockoutEndDateAsync(applicationUser, null);

				if (!unlockResult.Succeeded)
					throw new InvalidOperationException($"Failed to unlock user '{userName}'. Errors: {string.Join(", ", unlockResult.Errors.Select(e => e.Description))}");
			}

			var lockoutEnabled = await _userManager.SetLockoutEnabledAsync(applicationUser, true);

			if (!lockoutEnabled.Succeeded)
				throw new InvalidOperationException($"Failed to enable lockout for user '{userName}'.");

			return true;
		}
		#endregion

		#region Handle user roles

		/// <summary>
		/// Adds and removes authorization roles on a user.
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="userRole"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="KeyNotFoundException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public async Task<bool> HandleUserRolesAsync(User user)
		{
			if (user is null)
				throw new ArgumentNullException(nameof(user), "User cannot be empty.");

			var applicationUser = await _userManager.FindByNameAsync(user.UserName);

			if (applicationUser is null)
				throw new KeyNotFoundException($"User '{user.UserName}' not found.");

			if (user.Roles is not null)
			{
				var currentRoles = await _userManager.GetRolesAsync(applicationUser);

				var rolesToAdd = user.Roles.Except(currentRoles).ToList();
				var rolesToRemove = currentRoles.Except(user.Roles).ToList();

				if (rolesToAdd.Count > 0)
				{
					// Add roles
					var addRolesResult = await _userManager.AddToRolesAsync(applicationUser, rolesToAdd);
					
					if (!addRolesResult.Succeeded)
						throw new Exception($"Failed to add roles to user '{user.UserName}'. Errors: {string.Join(", ", addRolesResult.Errors.Select(e => e.Description))}");
				}

				if (rolesToRemove.Count > 0)
				{
					// Remove roles
					var removeRolesResult = await _userManager.RemoveFromRolesAsync(applicationUser, rolesToRemove);

					if (!removeRolesResult.Succeeded)
						throw new Exception($"Failed to remove roles from user '{user.UserName}'. Errors: {string.Join(", ", removeRolesResult.Errors.Select(e => e.Description))}");
				}
			}

			return true;
		}
		#endregion
	}
}
