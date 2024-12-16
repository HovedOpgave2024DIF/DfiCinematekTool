using System.ComponentModel.DataAnnotations.Schema;

namespace DfiCinematekTool.Domain.Entities
{
	[Table("Event")]
	public class Event
	{
		[Column("Id")]
		public int Id { get; set; }

		[Column("Title")]
		public string Title { get; set; } = string.Empty;

		[Column("DateId")]
		public int DateId { get; set; }

		[Column("Date")]
		public DateTime Date { get; set; }

		[Column("Screen")]
		public string Screen { get; set; } = string.Empty;

		[Column("DurationInMinutes")]
		public int DurationInMinutes { get; set; } = 0;

		[Column("Owner")]
		public string Owner { get; set; } = string.Empty;

		[Column("OwnerEmail")]
		public string OwnerEmail { get; set; } = string.Empty;

		[Column("EventType")]
		public string EventType { get; set; } = string.Empty;

		[Column("IsEvent")]
		public bool IsEvent { get; set; }

		[Column("IsRooftop")]
		public bool IsRooftop { get; set; }

		[Column("Published")]
		public bool? Published { get; set; }

		[Column("Abbriviation")]
		public string? Abbriviation { get; set; } = string.Empty;

		public ICollection<Film>? Films { get; set; } = new List<Film>();
		public ICollection<FilmStatus>? FilmStatuses { get; set; } = new List<FilmStatus>();
	}
}