using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieHub.Migrations
{
    public partial class change_genreEnum_to_Name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenreEnum",
                table: "Genre");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Genre",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Genre");

            migrationBuilder.AddColumn<int>(
                name: "GenreEnum",
                table: "Genre",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
