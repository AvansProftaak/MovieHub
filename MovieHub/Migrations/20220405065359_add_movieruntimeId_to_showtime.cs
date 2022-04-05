using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieHub.Migrations
{
    public partial class add_movieruntimeId_to_showtime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MovieRuntimeId",
                table: "Showtime",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Showtime_MovieRuntimeId",
                table: "Showtime",
                column: "MovieRuntimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Showtime_MovieRuntime_MovieRuntimeId",
                table: "Showtime",
                column: "MovieRuntimeId",
                principalTable: "MovieRuntime",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Showtime_MovieRuntime_MovieRuntimeId",
                table: "Showtime");

            migrationBuilder.DropIndex(
                name: "IX_Showtime_MovieRuntimeId",
                table: "Showtime");

            migrationBuilder.DropColumn(
                name: "MovieRuntimeId",
                table: "Showtime");
        }
    }
}
