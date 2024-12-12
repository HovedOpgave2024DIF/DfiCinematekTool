using DfiCinematekTool.Domain.Enums;

namespace DfiCinematekTool.Mapper
{
	public static class UserRoleExtension
	{
		public static UserRole ConvertUserRoleEnumToUserRole(this UserRoleEnum userRoleEnum)
		{
			return new UserRole
			{
				Role = userRoleEnum.ToString(),
				Value = userRoleEnum.ToString()
			};
		}
	}

	public class UserRole
	{
		public string Role { get; set; } = string.Empty;
		public string Value { get; set; } = string.Empty;
	}
}
