using DfiCinematekTool.Application.Interfaces;
using DfiCinematekTool.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
