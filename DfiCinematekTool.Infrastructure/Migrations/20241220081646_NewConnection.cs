using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfiCinematekTool.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilmStatus_Event_EventId",
                table: "FilmStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_FilmStatus_Film_FilmId",
                table: "FilmStatus");

            migrationBuilder.AlterColumn<int>(
                name: "FilmId",
                table: "FilmStatus",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "FilmStatus",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FilmStatus_Event_EventId",
                table: "FilmStatus",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FilmStatus_Film_FilmId",
                table: "FilmStatus",
                column: "FilmId",
                principalTable: "Film",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilmStatus_Event_EventId",
                table: "FilmStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_FilmStatus_Film_FilmId",
                table: "FilmStatus");

            migrationBuilder.AlterColumn<int>(
                name: "FilmId",
                table: "FilmStatus",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "FilmStatus",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_FilmStatus_Event_EventId",
                table: "FilmStatus",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FilmStatus_Film_FilmId",
                table: "FilmStatus",
                column: "FilmId",
                principalTable: "Film",
                principalColumn: "Id");
        }
    }
}
