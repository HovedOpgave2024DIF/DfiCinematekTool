using DfiCinematekTool.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace DfiCinematekTool.Infrastructure.Seeds
{
	public static class UserAndRoles
	{
		public static async Task SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			await SeedRoles(roleManager);
			await SeedUsers(userManager);
		}

		private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
		{
			List<IdentityRole> roles = new()
			{
				new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
				new IdentityRole { Name = "User", NormalizedName = "USER" },
				new IdentityRole { Name = "Administrator", NormalizedName = "ADMINISTRATOR" },
				new IdentityRole { Name = "Operatør", NormalizedName = "OPERATØR" },
				new IdentityRole { Name = "Programredaktør", NormalizedName = "PROGRAMREDAKTØR" },
			};

			foreach (var role in roles)
			{
				if (!await roleManager.RoleExistsAsync(role.Name ?? ""))
				{
					await roleManager.CreateAsync(role);
				}
			}
		}

		private static async Task SeedUsers(UserManager<ApplicationUser> userManager)
		{
			List<ApplicationUser> users = new()
			{
				new ApplicationUser
				{
					UserName = "admin@dfi.dk",
					NormalizedUserName = "ADMIN@DFI.DK",
					Email = "admin@dfi.dk",
					NormalizedEmail = "ADMIN@DFI.DK",
					EmailConfirmed = true
				},
				new ApplicationUser
				{
					UserName = "user@dfi.dk",
					NormalizedUserName = "USER@DFI.DK",
					Email = "user@dfi.dk",
					NormalizedEmail = "USER@DFI.DK",
					EmailConfirmed = true,
				},
				new ApplicationUser
				{
					UserName = "jamora",
					NormalizedUserName = "JAMORA",
					Email = "jamora@dfi.dk",
					NormalizedEmail = "JAMORA@DFI.DK",
					EmailConfirmed = true,
				},
				new ApplicationUser
				{
					UserName = "JohnSmith",
					NormalizedUserName = "JOHNSMITH",
					Email = "johnsmith@dfi.dk",
					NormalizedEmail = "JOHNSMITH@DFI.DK",
					EmailConfirmed = true,
				},
				new ApplicationUser
				{
					UserName = "JaneSmith",
					NormalizedUserName = "JANESMITH",
					Email = "janesmith@dfi.dk",
					NormalizedEmail = "JANESMITH@DFI.DK",
					EmailConfirmed = true,
				}
			};

			string[] roles = ["Admin", "User"];

			foreach (var user in users)
			{
				if (await userManager.FindByEmailAsync(user.Email ?? string.Empty) == null)
				{
					var result = await userManager.CreateAsync(user, "Password123!");

					if (result.Succeeded)
					{
						await userManager.AddToRoleAsync(
							user,
							user.Email == "admin@dfi.dk" ? roles[0] : roles[1]
						);
					}
					else
					{
						throw new InvalidOperationException($"User with ID {user.Id} does not exist in the database.");
					}
				}
			}
		}
	}
}
