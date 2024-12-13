using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication3.Migrations
{
    /// <inheritdoc />
    public partial class Add3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArtistId",
                table: "Exhibitions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exhibitions_ArtistId",
                table: "Exhibitions",
                column: "ArtistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exhibitions_Artists_ArtistId",
                table: "Exhibitions",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exhibitions_Artists_ArtistId",
                table: "Exhibitions");

            migrationBuilder.DropIndex(
                name: "IX_Exhibitions_ArtistId",
                table: "Exhibitions");

            migrationBuilder.DropColumn(
                name: "ArtistId",
                table: "Exhibitions");
        }
    }
}
