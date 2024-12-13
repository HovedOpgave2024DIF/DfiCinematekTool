using DfiCinematekTool.Domain.Interfaces;
using DfiCinematekTool.Infrastructure.Context;
using DfiCinematekTool.Infrastructure.Identity;
using DfiCinematekTool.Infrastructure.Interfaces;
using DfiCinematekTool.Infrastructure.Repositories;
using DfiCinematekTool.Infrastructure.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DfiCinematekTool.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services)
		{
			services.AddScoped<IEventRepository, EventRepository>();
			services.AddScoped<IFilmRepository, FilmRepository>();
			services.AddScoped<IFilmStatusRepository, FilmStatusRepository>();
			services.AddScoped<IUserRepository, ApplicationUserRepository>();
			services.AddScoped<IAuthorization, ApplicationUserAuthorization>();
			services.AddScoped<ApplicationUserAuthorization>();
			return services;
		}


		public static async Task UseSeedUserAndRoleDataAsync(this IServiceProvider serviceProvider)
		{
			using var scope = serviceProvider.CreateScope();

			try
			{
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
				var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

				await UserAndRoles.SeedData(userManager, roleManager);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error seeding user data: {ex.Message}");
			}
		}

		public static async Task UseSeedFilmsAndEventDataAsync(this IServiceProvider serviceProvider)
		{
			using var scope = serviceProvider.CreateScope();

			try
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<CinematekDbContext>();

				await FilmsAndEvents.SeedData(dbContext);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error seeding film and event data: {ex.Message}");
			}
		}
	}
}
