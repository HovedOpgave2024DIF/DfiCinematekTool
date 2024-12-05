using DfiCinematekTool.Infrastructure.Identity;
using DfiCinematekTool.Infrastructure.Interfaces;
using DfiCinematekTool.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace DfiCinematekTool.Infrastructure.Repositories
{
	public class ApplicationUserAuthorization : IAuthorization
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public ApplicationUserAuthorization(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task<bool> HandleUserLockoutAsync(string userName, bool isLocked)
		{
			if (string.IsNullOrWhiteSpace(userName))
				throw new ArgumentNullException(nameof(userName), "User name cannot be empty.");

			var applicationUser = await _userManager.FindByNameAsync(userName);

			if (applicationUser is null)
				throw new KeyNotFoundException($"User '{userName}' not found.");

			if (isLocked)
			{
				var lockoutResult = await _userManager.SetLockoutEndDateAsync(applicationUser, DateTimeOffset.MaxValue);

				if (!lockoutResult.Succeeded)
					throw new InvalidOperationException($"Failed to lock out user '{userName}'. Errors: {string.Join(", ", lockoutResult.Errors.Select(e => e.Description))}");
			}
			else
			{
				var unlockResult = await _userManager.SetLockoutEndDateAsync(applicationUser, null);

				if (!unlockResult.Succeeded)
					throw new InvalidOperationException($"Failed to unlock user '{userName}'. Errors: {string.Join(", ", unlockResult.Errors.Select(e => e.Description))}");
			}

			var lockoutEnabled = await _userManager.SetLockoutEnabledAsync(applicationUser, true);
			if (!lockoutEnabled.Succeeded)
				throw new InvalidOperationException($"Failed to enable lockout for user '{userName}'.");

			return true;
		}

		public async Task<bool> HandleUserRolesAsync(string userName, UserRole userRole)
		{
			if (!string.IsNullOrWhiteSpace(userName))
				throw new ArgumentNullException(nameof(userName), "User name cannot be empty.");

			var applicationUser = await _userManager.FindByNameAsync(userName);

			if (applicationUser is null)
				throw new KeyNotFoundException($"User '{userName}' not found.");

			var currentRoles = await _userManager.GetRolesAsync(applicationUser);

			// Remove all current roles
			if (currentRoles.Any())
			{
				var removeRolesResult = await _userManager.RemoveFromRolesAsync(applicationUser, currentRoles);
				if (!removeRolesResult.Succeeded)
					throw new InvalidOperationException($"Failed to remove roles from user '{userName}'. Errors: {string.Join(", ", removeRolesResult.Errors.Select(e => e.Description))}");
			}

			// Add the new role
			var newRole = userRole.ToString();
			var addRoleResult = await _userManager.AddToRoleAsync(applicationUser, newRole);

			if (!addRoleResult.Succeeded)
				throw new InvalidOperationException($"Failed to assign role '{newRole}' to user '{userName}'. Errors: {string.Join(", ", addRoleResult.Errors.Select(e => e.Description))}");

			return true;
		}

		public Task<bool> HandleUserPermissionsAsync()
		{
			throw new NotImplementedException();
		}
	}
}
