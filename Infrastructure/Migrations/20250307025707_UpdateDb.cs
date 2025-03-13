using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Showtimes_ShowtimeId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Showtimes_Movies_MovieId",
                table: "Showtimes");

            migrationBuilder.DropForeignKey(
                name: "FK_Showtimes_Rooms_RoomId",
                table: "Showtimes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Showtimes",
                table: "Showtimes");

            migrationBuilder.DropIndex(
                name: "IX_Showtimes_MovieId",
                table: "Showtimes");

            migrationBuilder.DropIndex(
                name: "IX_Showtimes_RoomId",
                table: "Showtimes");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "Showtimes");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Showtimes");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Showtimes");

            migrationBuilder.DropColumn(
                name: "TicketPrice",
                table: "Showtimes");

            migrationBuilder.RenameTable(
                name: "Showtimes",
                newName: "ShowTimes");

            migrationBuilder.RenameColumn(
                name: "ShowtimeId",
                table: "Bookings",
                newName: "ShowId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_ShowtimeId",
                table: "Bookings",
                newName: "IX_Bookings_ShowId");

            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "ShowTimes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShowTimes",
                table: "ShowTimes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ShowDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowDays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    ShowDayId = table.Column<int>(type: "int", nullable: false),
                    ShowTimeId = table.Column<int>(type: "int", nullable: false),
                    TicketPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shows_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shows_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shows_ShowDays_ShowDayId",
                        column: x => x.ShowDayId,
                        principalTable: "ShowDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shows_ShowTimes_ShowTimeId",
                        column: x => x.ShowTimeId,
                        principalTable: "ShowTimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shows_MovieId",
                table: "Shows",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Shows_RoomId",
                table: "Shows",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Shows_ShowDayId",
                table: "Shows",
                column: "ShowDayId");

            migrationBuilder.CreateIndex(
                name: "IX_Shows_ShowTimeId",
                table: "Shows",
                column: "ShowTimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Shows_ShowId",
                table: "Bookings",
                column: "ShowId",
                principalTable: "Shows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Shows_ShowId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "Shows");

            migrationBuilder.DropTable(
                name: "ShowDays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShowTimes",
                table: "ShowTimes");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "ShowTimes");

            migrationBuilder.RenameTable(
                name: "ShowTimes",
                newName: "Showtimes");

            migrationBuilder.RenameColumn(
                name: "ShowId",
                table: "Bookings",
                newName: "ShowtimeId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_ShowId",
                table: "Bookings",
                newName: "IX_Bookings_ShowtimeId");

            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "Showtimes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Showtimes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Showtimes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TicketPrice",
                table: "Showtimes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Showtimes",
                table: "Showtimes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Showtimes_MovieId",
                table: "Showtimes",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Showtimes_RoomId",
                table: "Showtimes",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Showtimes_ShowtimeId",
                table: "Bookings",
                column: "ShowtimeId",
                principalTable: "Showtimes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Showtimes_Movies_MovieId",
                table: "Showtimes",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Showtimes_Rooms_RoomId",
                table: "Showtimes",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
