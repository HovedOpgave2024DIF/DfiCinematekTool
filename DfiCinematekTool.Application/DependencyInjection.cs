using DfiCinematekTool.Application.Interfaces;
using DfiCinematekTool.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DfiCinematekTool.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddScoped<IUserService, UserService>();
			return services;
		}
	}
}
