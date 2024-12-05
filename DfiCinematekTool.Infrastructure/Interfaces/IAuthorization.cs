using DfiCinematekTool.Domain.Enums;

namespace DfiCinematekTool.Infrastructure.Interfaces
{
	public interface IAuthorization
	{
		Task<bool> HandleUserLockoutAsync(string userName, bool isLocked);
		Task<bool> HandleUserRolesAsync(string userName, UserRole userRole);
		Task<bool> HandleUserPermissionsAsync();
	}
}
