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
