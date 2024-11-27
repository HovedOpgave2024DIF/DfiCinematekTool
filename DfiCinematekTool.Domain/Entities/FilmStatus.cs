using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DfiCinematekTool.Domain.Entities
{
	[Table(name: "FilmStatus")]
	public class FilmStatus
	{
		[Column(name: "Id")]
		public int Id { get; set; }

		[Column(name: "EventId")]
		public int? EventId { get; set; }

		[Column(name: "FilmId")]
		public int? FilmId { get; set; }

		[Column(name: "ReceivedDate")]
		public DateTime DateTime { get; set; }

		[Column(name: "CheckedDate")]
		public DateTime CheckedDate { get; set; }

		[Column(name: "PreparedDate")]
		public DateTime PreparedDate { get; set; }

		[Column(name:"Comment")]
		public string Comment { get; set; } = string.Empty;

		[Column(name: "HasKey")]
		public bool HasKey { get; set; }
	}
}
