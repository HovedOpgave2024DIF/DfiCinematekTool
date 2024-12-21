using DfiCinematekTool.Domain.Entities;

namespace DfiCinematekTool.Infrastructure.Interfaces
{
	public interface IAuthorization
	{
		Task<bool> HandleUserLockoutAsync(string userName, bool isLocked);
		Task<bool> HandleUserRolesAsync(User user);
	}
}
