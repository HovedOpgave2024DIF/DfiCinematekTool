using System.ComponentModel.DataAnnotations.Schema;

namespace DfiCinematekTool.Domain.Entities
{
	[Table("Film")]
	public class Film
	{
		[Column("Id")]
		public int Id { get; set; }

		[Column("Title")]
		public string Title { get; set; } = string.Empty;

		[Column("OriginalTitle")]
		public string OriginalTitle { get; set; } = string.Empty;

		[Column("Format")]
		public string Format { get; set; } = string.Empty;

		[Column("DurationInMinutes")]
		public int DurationInMinutes { get; set; }

		[Column("SortOrder")]
		public int SortOrder { get; set; }

		public ICollection<Event>? Events { get; set; } = new List<Event>();
	}
}
