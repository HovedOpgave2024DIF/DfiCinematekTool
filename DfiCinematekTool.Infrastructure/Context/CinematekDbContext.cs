using DfiCinematekTool.Domain.Entities;
using DfiCinematekTool.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DfiCinematekTool.Infrastructure.Context
{
	public class CinematekDbContext : IdentityDbContext<ApplicationUser>
	{
		public CinematekDbContext(DbContextOptions<CinematekDbContext> options) : base(options) 
		{ 

		}

		DbSet<Film> Films { get; set; }
	}
}
