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
					UserName = "Admin",
					NormalizedUserName = "ADMIN@DFI.DK",
					Email = "admin@dfi.dk",
					NormalizedEmail = "ADMIN@DFI.DK",
					EmailConfirmed = true
				},
				new ApplicationUser
				{
					UserName = "Jamora",
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
				}
			};

			string[] roles = ["Administrator", "Operatør", "Programredaktør"];
			string[] usernames = ["Admin", "Jamora", "JohnSmith"];

            for (int i = 0; i < users.Count; i++)
            {
                var user = users[i];
                var role = roles[i];

                if (await userManager.FindByNameAsync(user.UserName ?? string.Empty) == null)
                {
                    var result = await userManager.CreateAsync(user, "Password123!");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                }
            }
        }
	}
}
