using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieHub.Migrations
{
    public partial class EnumFix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Genre",
                newName: "GenreEnum");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Order",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "GenreEnum",
                table: "Genre",
                newName: "Category");
        }
    }
}
