using DfiCinematekTool.Domain.Entities;
using DfiCinematekTool.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DfiCinematekTool.Infrastructure.Context
{
	public class CinematekDbContext : IdentityDbContext<ApplicationUser>
	{
		public CinematekDbContext(DbContextOptions<CinematekDbContext> options) : base(options) 
		{ 

		}

		public DbSet<Film> Films { get; set; } = default!;
		public DbSet<Event> Events { get; set; } = default!;
		public DbSet<FilmStatus> FilmStatuses { get; set; } = default!;
	}
}
