using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShowV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shows_ShowDays_ShowDayId",
                table: "Shows");

            migrationBuilder.DropForeignKey(
                name: "FK_Shows_ShowTimes_ShowTimeId",
                table: "Shows");

            migrationBuilder.DropTable(
                name: "ShowDays");

            migrationBuilder.DropTable(
                name: "ShowTimes");

            migrationBuilder.DropIndex(
                name: "IX_Shows_ShowDayId",
                table: "Shows");

            migrationBuilder.DropIndex(
                name: "IX_Shows_ShowTimeId",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "ShowDayId",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "ShowTimeId",
                table: "Shows");

            migrationBuilder.AddColumn<DateTime>(
                name: "ShowDay",
                table: "Shows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "ShowTime",
                table: "Shows",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowDay",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "ShowTime",
                table: "Shows");

            migrationBuilder.AddColumn<int>(
                name: "ShowDayId",
                table: "Shows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShowTimeId",
                table: "Shows",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                name: "ShowTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowTimes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shows_ShowDayId",
                table: "Shows",
                column: "ShowDayId");

            migrationBuilder.CreateIndex(
                name: "IX_Shows_ShowTimeId",
                table: "Shows",
                column: "ShowTimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shows_ShowDays_ShowDayId",
                table: "Shows",
                column: "ShowDayId",
                principalTable: "ShowDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shows_ShowTimes_ShowTimeId",
                table: "Shows",
                column: "ShowTimeId",
                principalTable: "ShowTimes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
