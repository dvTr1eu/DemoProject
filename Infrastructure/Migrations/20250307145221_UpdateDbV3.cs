using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CinemaId",
                table: "Shows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Shows_CinemaId",
                table: "Shows",
                column: "CinemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shows_Cinemas_CinemaId",
                table: "Shows",
                column: "CinemaId",
                principalTable: "Cinemas",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shows_Cinemas_CinemaId",
                table: "Shows");

            migrationBuilder.DropIndex(
                name: "IX_Shows_CinemaId",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "CinemaId",
                table: "Shows");
        }
    }
}
