using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieHub.Migrations
{
    public partial class DBRESTOREfixdoubledecimalproblemv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Tickettype",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "CateringPackage",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Tickettype",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "CateringPackage",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }
    }
}
