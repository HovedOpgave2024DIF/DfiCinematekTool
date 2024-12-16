using DfiCinematekTool.Domain.Entities;
using DfiCinematekTool.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DfiCinematekTool.Infrastructure.Seeds
{
	public static class FilmsAndEvents
	{
		public static async Task SeedData(CinematekDbContext dbContext)
		{
			await SeedFilms(dbContext);
			await SeedEvents(dbContext);
			await SeedFilmStatus(dbContext);
		}

		private static async Task SeedFilms(CinematekDbContext dbContext)
		{
			List<Film> films = new()
			{
				new Film { Title = "Den lille fugl og larven", Format = "DCP", DurationInMinutes = 4, SortOrder = 1 },
				new Film { Title = "Forårsfest", Format = "DCP", DurationInMinutes = 5, SortOrder = 2 },
				new Film { Title = "Astons gaver", OriginalTitle = "Astons presenter", Format = "DCP", DurationInMinutes = 9, SortOrder = 3 },
				new Film { Title = "En lille smule", OriginalTitle = "Bara Lite", Format = "DCP", DurationInMinutes = 9, SortOrder = 4 },
				new Film { Title = "Min oldefars historier - Ridderne", Format = "DCP", DurationInMinutes = 9, SortOrder = 5 },
				new Film { Title = "Pipfugle", Format = "DCP", DurationInMinutes = 9, SortOrder = 6 },
				
				new Film { Title = "Næsehornsdans", OriginalTitle = "Nashorntwist", Format = "DCP", DurationInMinutes = 3, SortOrder = 7 },
				new Film { Title = "Mægtige maskiner – Tårnkran", Format = "DCP", DurationInMinutes = 6, SortOrder = 8 },
				new Film { Title = "Peter Pix - Violinen", Format = "DCP", DurationInMinutes = 2, SortOrder = 9 },
				new Film { Title = "Cirkeline – Månen er en gul ost", Format = "DCP", DurationInMinutes = 13, SortOrder = 10 },
				new Film { Title = "Sallies historier – Hr. Wilders stjerne", Format = "DCP", DurationInMinutes = 7, SortOrder = 11 },
				new Film { Title = "Rollinger - Raketten", Format = "DCP", DurationInMinutes = 8, SortOrder = 12 },

				new Film { Title = "Totem - Ama og det magiske hulepindsvin", OriginalTitle = "Totem", Format = "DCP",DurationInMinutes = 98, SortOrder = 1 },

				new Film { Title = "Morderenglen", OriginalTitle = "El ángel exterminador", Format = "DCP", DurationInMinutes = 95, SortOrder = 1 },

				new Film { Title = "Blue Velvet", OriginalTitle = "", Format = "DCP 4K", DurationInMinutes = 120, SortOrder = 1 },

				new Film { Title = "Jerusalem", OriginalTitle = "", Format = "35mm", DurationInMinutes = 168, SortOrder = 1 }
			};


			foreach (var film in films)
			{
				
				if (!await dbContext.Films.AnyAsync(f => f.Title == film.Title))
				{
					await dbContext.Films.AddAsync(film);
				}
			}

			await dbContext.SaveChangesAsync();
		}

		private static async Task SeedEvents(CinematekDbContext dbContext)
		{
			List<Event> events = new()
			{
				new Event
				{
					Title = "Salaam Film Festival",
					DateId = 192934,
					Date = new DateTime(2024, 11, 15, 9, 0, 0),
					Screen = "Foyer",
					DurationInMinutes = 360,
					Owner = "FILMHUSET\\catharinab",
					OwnerEmail = "catharinab@dfi.dk",
					EventType = "Ekstern booking",
					IsEvent = false,
					IsRooftop = false,
					Published = false,
					Abbriviation = null,
					Films = []
				},
				new Event
				{
					Title = "Børnebiffen 3-5 år (1)",
					DateId = 195213,
					Date = new DateTime(2024, 11, 15, 10, 0, 0),
					Screen = "Asta",
					DurationInMinutes = 45,
					Owner = "FILMHUSET\\christianh",
					EventType = "Serie",
					IsEvent = true,
					IsRooftop = false,
					Published = true,
					Abbriviation = "BORN",
					Films = new List<Film>
					{
						new Film { Id = 1 },
						new Film { Id = 2 },
						new Film { Id = 3 },
						new Film { Id = 4 },
						new Film { Id = 5 },
						new Film { Id = 6 },
					}
				},
				new Event
				{
					Title = "Børnebiffen 5-7 år",
					DateId = 195219,
					Date = new DateTime(2024, 11, 16, 12, 30, 0),
					Screen = "Carl",
					DurationInMinutes = 50,
					Owner = "FILMHUSET\\christianh",
					OwnerEmail = "",
					EventType = "Serie",
					IsEvent = true,
					IsRooftop = false,
					Published = true,
					Abbriviation = "BORN",
					Films = new List<Film>
					{
						new Film { Id = 7 },
						new Film { Id = 8 },
						new Film { Id = 9 },
						new Film { Id = 10 },
						new Film { Id = 11 },
						new Film { Id = 12 },
					}
				},
				new Event
				{
					Title = "Totem - Ama og det magiske hulepindsvin",
					DateId = 196589,
					Date = new DateTime(2024, 11, 16, 14, 15, 0),
					Screen = "Carl",
					DurationInMinutes = 98,
					Owner = "FILMHUSET\\tobiash",
					OwnerEmail = "tobiash@dfi.dk",
					EventType = "Serie",
					IsEvent = false,
					IsRooftop = false,
					Published = true,
					Abbriviation = "EFTE",
					Films = new List<Film>
					{
						new Film{ Id = 13 }
					}
				},
				new Event
				{
					Title = "Morderenglen",
					DateId = 197009,
					Date = new DateTime(2024, 11, 17, 16, 0, 0),
					Screen = "Asta",
					DurationInMinutes = 95,
					Owner = "PROD\\B034422",
					OwnerEmail = "",
					EventType = "Serie",
					IsEvent = false,
					IsRooftop = false,
					Published = true,
					Abbriviation = "SURR",
					Films = new List<Film>
					{
						new Film { Id = 14 }
					}
				},
				new Event
				{
					Title = "Blue Velvet",
					DateId = 196826,
					Date = new DateTime(2024, 11, 17, 16, 30, 0),
					Screen = "Carl",
					DurationInMinutes = 120,
					Owner = "FILMHUSET\\mortent",
					OwnerEmail = "mortent@dfi.dk",
					EventType = "Serie",
					IsEvent = false,
					IsRooftop = false,
					Published = true,
					Abbriviation = "ØNSK",
					Films = new List<Film>
					{
						new Film { Id = 15 }
					}
				},
				new Event
				{
					Title = "East by (JA/TLH)",
					DateId = 196413,
					Date = new DateTime(2024, 11, 17, 18, 0, 0),
					Screen = "Asta Bar",
					DurationInMinutes = 180,
					Owner = "FILMHUSET\\tobiash",
					OwnerEmail = "tobiash@dfi.dk",
					EventType = "Ekstern booking",
					IsEvent = false,
					IsRooftop = false,
					Published = false,
					Abbriviation = null,
					Films = []
				},
				new Event
				{
					Title = "Jerusalem",
					DateId = 197292,
					Date = new DateTime(2024, 11, 17, 19, 0, 0),
					Screen = "Carl",
					DurationInMinutes = 168,
					Owner = "FILMHUSET\\tobiash",
					OwnerEmail = "tobiash@dfi.dk",
					EventType = "Serie",
					IsEvent = false,
					IsRooftop = false,
					Published = true,
					Abbriviation = "NORD",
					Films = new List<Film>
					{
						new Film { Id = 16 }
					}
				}
			};

			foreach (var ev in events)
			{
				if (!await dbContext.Events.AnyAsync(e => e.Title == ev.Title))
				{
					var attachedFilms = new List<Film>();

					if(ev.Films is not null)
					{
						foreach (var film in ev.Films)
						{
							var existingFilm = await dbContext.Films.FindAsync(film.Id);
							if (existingFilm is not null)
							{
								attachedFilms.Add(existingFilm);
							}
							else
							{
								throw new InvalidOperationException($"Film with ID {film.Id} does not exist in the database.");
							}
						}

						ev.Films = attachedFilms;

						await dbContext.Events.AddAsync(ev);
					}
				}
			}

			await dbContext.SaveChangesAsync();
		}

		private static async Task SeedFilmStatus(CinematekDbContext dbContext)
		{
			List<FilmStatus> filmStatuses = new()
			{
				new FilmStatus()
				{
					EventId = 2,
					FilmId = 1,
					OrderDate = new DateTime(2024,12,2),
					ReceivedDate = new DateTime(2024,12,2),
					CheckedDate = new DateTime(2024,12,2),
					PreparedDate =new DateTime(2024,12,2),
					Comment = "Test Comment 1",
					HasKey = true
				},
				new FilmStatus()
				{
					EventId = 2,
					FilmId = 2,
					OrderDate = new DateTime(2024,12,2),
					ReceivedDate = new DateTime(2024,12,2),
					CheckedDate = new DateTime(2024,12,2),
					PreparedDate = new DateTime(2024,12,2),
					Comment = "Test Comment 2",
					HasKey = true
				},
				new FilmStatus()
				{
					EventId = 2,
					FilmId = 3,
					OrderDate = new DateTime(2024,12,2),
					ReceivedDate = new DateTime(2024,12,2),
					CheckedDate = new DateTime(2024,12,2),
					PreparedDate =new DateTime(2024,12,2),
					Comment = "Test Comment 3",
					HasKey = true
				},
				new FilmStatus()
				{
					EventId = 2,
					FilmId = 4,
					OrderDate = new DateTime(2024,12,2),
					ReceivedDate = new DateTime(2024,12,2),
					CheckedDate = new DateTime(2024,12,2),
					PreparedDate = new DateTime(2024,12,2),
					Comment = "Test Comment 4",
					HasKey = true
				},
				new FilmStatus()
				{
					EventId = 2,
					FilmId = 5,
					OrderDate = new DateTime(2024,12,2),
					ReceivedDate = new DateTime(2024,12,2),
					CheckedDate = new DateTime(2024,12,2),
					PreparedDate =new DateTime(2024,12,2),
					Comment = "Test Comment 5",
					HasKey = false
				},
				new FilmStatus()
				{
					EventId = 2,
					FilmId = 6,
					OrderDate = new DateTime(2024,12,2),
					ReceivedDate = new DateTime(2024,12,2),
					CheckedDate = new DateTime(2024,12,2),
					PreparedDate = new DateTime(2024,12,2),
					Comment = "Test Comment 6",
					HasKey = false
				},
				
				new FilmStatus()
				{
					EventId = 3,
					FilmId = 7,
					OrderDate = new DateTime(2024,12,2),
					ReceivedDate = new DateTime(2024,12,2),
					CheckedDate = new DateTime(2024,12,2),
					PreparedDate =new DateTime(2024,12,2),
					Comment = "Test Comment 1",
					HasKey = true
				},
				new FilmStatus()
				{
					EventId = 3,
					FilmId = 8,
					OrderDate = new DateTime(2024,12,2),
					ReceivedDate = new DateTime(2024,12,2),
					CheckedDate = new DateTime(2024,12,2),
					PreparedDate = new DateTime(2024,12,2),
					Comment = "Test Comment 2",
					HasKey = true
				},
				new FilmStatus()
				{
					EventId = 3,
					FilmId = 9,
					OrderDate = new DateTime(2024,12,2),
					ReceivedDate = new DateTime(2024,12,2),
					CheckedDate = new DateTime(2024,12,2),
					PreparedDate =new DateTime(2024,12,2),
					Comment = "Test Comment 3",
					HasKey = true
				},
				new FilmStatus()
				{
					EventId = 3,
					FilmId = 10,
					OrderDate = new DateTime(2024,12,2),
					ReceivedDate = new DateTime(2024,12,2),
					CheckedDate = new DateTime(2024,12,2),
					PreparedDate = new DateTime(2024,12,2),
					Comment = "Test Comment 4",
					HasKey = true
				},
				new FilmStatus()
				{
					EventId = 3,
					FilmId = 11,
					OrderDate = new DateTime(2024,12,2),
					ReceivedDate = new DateTime(2024,12,2),
					CheckedDate = new DateTime(2024,12,2),
					PreparedDate =new DateTime(2024,12,2),
					Comment = "Test Comment 5",
					HasKey = false
				},
				new FilmStatus()
				{
					EventId = 3,
					FilmId = 12,
					OrderDate = new DateTime(2024,12,2),
					ReceivedDate = new DateTime(2024,12,2),
					CheckedDate = new DateTime(2024,12,2),
					PreparedDate = new DateTime(2024,12,2),
					Comment = "Test Comment 6",
					HasKey = false
				},

				new FilmStatus()
				{
					EventId = 4,
					FilmId = 13,
					OrderDate = new DateTime(2024,12,2),
					ReceivedDate = new DateTime(2024,12,2),
					CheckedDate = new DateTime(2024,12,2),
					PreparedDate = new DateTime(2024,12,2),
					Comment = "Test Comment 7",
					HasKey = false
				},

				new FilmStatus()
				{
					EventId = 5,
					FilmId = 14,
					OrderDate = new DateTime(2024,12,2),
					ReceivedDate = new DateTime(2024,12,2),
					CheckedDate = new DateTime(2024,12,2),
					PreparedDate = new DateTime(2024,12,2),
					Comment = "Test Comment 8",
					HasKey = false
				},

				new FilmStatus()
				{
					EventId = 6,
					FilmId = 15,
					OrderDate = new DateTime(2024,12,2),
					ReceivedDate = new DateTime(2024,12,2),
					CheckedDate = new DateTime(2024,12,2),
					PreparedDate = new DateTime(2024,12,2),
					Comment = "Test Comment 9",
					HasKey = false
				},

				new FilmStatus()
				{
					EventId = 8,
					FilmId = 16,
					OrderDate = new DateTime(2024,12,2),
					ReceivedDate = new DateTime(2024,12,2),
					CheckedDate = new DateTime(2024,12,2),
					PreparedDate = new DateTime(2024,12,2),
					Comment = "Test Comment 10",
					HasKey = false
				}

			};

			foreach (var fs in filmStatuses)
			{
				if (!await dbContext.FilmStatuses.AnyAsync(e => e.EventId == fs.EventId && e.FilmId != fs.FilmId))
				{
					await dbContext.FilmStatuses.AddAsync(fs);
				}
			}

			await dbContext.SaveChangesAsync();
		}
	}
}
