using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MovieHub.Migrations
{
    public partial class HallTableV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hall",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CinemaId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    Has3d = table.Column<bool>(type: "boolean", nullable: false),
                    DolbySurround = table.Column<bool>(type: "boolean", nullable: false),
                    WheelchairAccess = table.Column<bool>(type: "boolean", nullable: false),
                    RelaxChairs = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hall", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hall_Cinema_CinemaId",
                        column: x => x.CinemaId,
                        principalTable: "Cinema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hall_CinemaId",
                table: "Hall",
                column: "CinemaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hall");
        }
    }
}
