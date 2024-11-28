using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfiCinematekTool.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAllEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateId = table.Column<int>(type: "int", nullable: false),
                    Screen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DurationInMinutes = table.Column<int>(type: "int", nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEvent = table.Column<bool>(type: "bit", nullable: false),
                    IsRooftop = table.Column<bool>(type: "bit", nullable: false),
                    Published = table.Column<bool>(type: "bit", nullable: true),
                    Abbriviation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventFilm",
                columns: table => new
                {
                    EventsId = table.Column<int>(type: "int", nullable: false),
                    FilmsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventFilm", x => new { x.EventsId, x.FilmsId });
                    table.ForeignKey(
                        name: "FK_EventFilm_Event_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventFilm_Film_FilmsId",
                        column: x => x.FilmsId,
                        principalTable: "Film",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: true),
                    FilmId = table.Column<int>(type: "int", nullable: true),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PreparedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasKey = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilmStatus_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FilmStatus_Film_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Film",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventFilm_FilmsId",
                table: "EventFilm",
                column: "FilmsId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmStatus_EventId",
                table: "FilmStatus",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmStatus_FilmId",
                table: "FilmStatus",
                column: "FilmId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventFilm");

            migrationBuilder.DropTable(
                name: "FilmStatus");

            migrationBuilder.DropTable(
                name: "Event");
        }
    }
}
