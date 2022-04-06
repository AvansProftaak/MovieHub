using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieHub.Migrations
{
    public partial class reviewChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Cinema_CinemaId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Hall_HallId",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Review_CinemaId",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Review_HallId",
                table: "Review");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Review_CinemaId",
                table: "Review",
                column: "CinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_HallId",
                table: "Review",
                column: "HallId");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Cinema_CinemaId",
                table: "Review",
                column: "CinemaId",
                principalTable: "Cinema",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Hall_HallId",
                table: "Review",
                column: "HallId",
                principalTable: "Hall",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
