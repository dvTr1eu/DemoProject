using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookingSeat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingSeats",
                table: "BookingSeats");

            migrationBuilder.AddColumn<int>(
                name: "ShowTimeId",
                table: "BookingSeats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingSeats",
                table: "BookingSeats",
                columns: new[] { "BookId", "SeatId", "ShowTimeId" });

            migrationBuilder.CreateIndex(
                name: "IX_BookingSeats_ShowTimeId",
                table: "BookingSeats",
                column: "ShowTimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingSeats_ShowTimeDetails_ShowTimeId",
                table: "BookingSeats",
                column: "ShowTimeId",
                principalTable: "ShowTimeDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingSeats_ShowTimeDetails_ShowTimeId",
                table: "BookingSeats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingSeats",
                table: "BookingSeats");

            migrationBuilder.DropIndex(
                name: "IX_BookingSeats_ShowTimeId",
                table: "BookingSeats");

            migrationBuilder.DropColumn(
                name: "ShowTimeId",
                table: "BookingSeats");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingSeats",
                table: "BookingSeats",
                columns: new[] { "BookId", "SeatId" });
        }
    }
}
