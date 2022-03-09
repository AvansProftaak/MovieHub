using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieHub.Migrations
{
    public partial class DBRESTOREaddcateringpackagequantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "CateringPackage",
                newName: "Quantity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "CateringPackage",
                newName: "Amount");
        }
    }
}
