using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DfiCinematekTool.Domain.Entities
{
	[Table(name: "Film")]
	public class Film
	{
		[Column(name: "Id")]
		public int Id { get; set; }

		[Column(name: "Title")]
		public string Title { get; set; } = string.Empty;

		[Column(name: "OriginalTitle")]
		public string OriginalTitle { get; set; } = string.Empty;

		[Column(name: "Format")]
		public string Format { get; set; } = string.Empty;

		[Column(name: "DurationInMinutes")]
		public int DurationInMinutes { get; set; }

		[Column(name:"SortOrder")]
		public int SortOrder { get; set; }

	}
}
