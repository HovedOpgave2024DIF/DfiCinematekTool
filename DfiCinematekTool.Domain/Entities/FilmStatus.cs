using System.ComponentModel.DataAnnotations.Schema;

namespace DfiCinematekTool.Domain.Entities
{
	[Table("FilmStatus")]
	public class FilmStatus
	{
		[Column("Id")]
		public int Id { get; set; }

		[Column("EventId")]
		[ForeignKey(nameof(Event))]
		public int EventId { get; set; }

		[Column("FilmId")]
		[ForeignKey(nameof(Film))]
		public int FilmId { get; set; }

		public Event? Event { get; set; }

		public Film? Film { get; set; }

		[Column("OrderDate")]
		public DateTime OrderDate { get; set; }

		[Column("ReceivedDate")]
		public DateTime ReceivedDate { get; set; }

		[Column("CheckedDate")]
		public DateTime CheckedDate { get; set; }

		[Column("PreparedDate")]
		public DateTime PreparedDate { get; set; }

		[Column("Comment")]
		public string Comment { get; set; } = string.Empty;

		[Column("HasKey")]
		public bool HasKey { get; set; }
	}
}
