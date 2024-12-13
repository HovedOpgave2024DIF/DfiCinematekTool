namespace DfiCinematekTool.Domain.Entities
{
	public class User
	{
		public string Id { get; set; } = string.Empty;

		public string UserName { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;
		
		public string Password { get; set; } = string.Empty;

		public DateTimeOffset? Lockout { get; set; }
		
		public ICollection<string>? Roles { get; set; } = new List<string>();
	}
}
