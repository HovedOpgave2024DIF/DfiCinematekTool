using System.ComponentModel.DataAnnotations;

namespace DfiCinematekTool.Models
{
	public class LoginModel
	{
		[Required(ErrorMessage = "Angiv venligst brugernavn")]
		public string Username { get; set; } = string.Empty;

		[Required(ErrorMessage = "Angiv venligst password")]
		public string Password { get; set; } = string.Empty;
	}
}
