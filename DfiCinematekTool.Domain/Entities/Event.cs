using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DfiCinematekTool.Domain.Entities
{
	[Table(name: "Event")]
	public class Event
	{
		[Column(name: "Id")]
		public int Id { get; set; }

		[Column(name: "Title")]
		public string Title { get; set; } = string.Empty;

		[Column(name: "DateId")]
		public int DateId { get; set; }

		[Column(name: "Screen")]
		public string Screen { get; set; } = string.Empty;

		[Column(name: "DurationInMinutes")]
		public int DurationInMinutes { get; set; }

		[Column(name: "Owner")]
		public string Owner { get; set; } = string.Empty;

		[Column(name: "OwnerEmail")]
		public string OwnerEmail { get; set; } = string.Empty;

		[Column(name: "EventType")]
		public string EventType { get; set; } = string.Empty;

		[Column(name: "IsEvent")]
		public bool IsEvent { get; set; }

		[Column(name: "IsRooftop")]
		public bool IsRooftop { get; set; }

		[Column(name: "Published")]
		public bool Published { get; set; }

		[Column(name: "Abbriviation")]
		public string Abbriviation { get; set; } = string.Empty;
	}
}
