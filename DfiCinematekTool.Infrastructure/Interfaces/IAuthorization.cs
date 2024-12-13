using DfiCinematekTool.Domain.Enums;

namespace DfiCinematekTool.Infrastructure.Interfaces
{
	public interface IAuthorization
	{
		Task<bool> HandleUserLockoutAsync(string userName, bool isLocked);
		Task<bool> HandleUserRolesAsync(string userName, UserRoleEnum userRole);
		Task<bool> HandleUserPermissionsAsync();
	}
}
