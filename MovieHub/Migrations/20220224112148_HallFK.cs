using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieHub.Migrations
{
    public partial class HallFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Seat_HallId",
                table: "Seat",
                column: "HallId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_Hall_HallId",
                table: "Seat",
                column: "HallId",
                principalTable: "Hall",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seat_Hall_HallId",
                table: "Seat");

            migrationBuilder.DropIndex(
                name: "IX_Seat_HallId",
                table: "Seat");
        }
    }
}
