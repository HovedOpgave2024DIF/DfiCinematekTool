﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DfiCinematekTool.Domain.Entities
{
	public class User
	{
		public int Id { get; set; }

		public string UserName { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;

		public string Password { get; set; } = string.Empty;

		public string Role { get; set; } = string.Empty;
	}
}
