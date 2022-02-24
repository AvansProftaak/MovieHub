using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieHub.Migrations
{
    public partial class Fkv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Order",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CateringPackageId",
                table: "Order",
                column: "CateringPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ShowtimeId",
                table: "Order",
                column: "ShowtimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_CateringPackage_CateringPackageId",
                table: "Order",
                column: "CateringPackageId",
                principalTable: "CateringPackage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Showtime_UserId",
                table: "Order",
                column: "UserId",
                principalTable: "Showtime",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_ShowtimeId",
                table: "Order",
                column: "ShowtimeId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_CateringPackage_CateringPackageId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Showtime_UserId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_ShowtimeId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_CateringPackageId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_ShowtimeId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_UserId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Order",
                newName: "CustomerId");
        }
    }
}
