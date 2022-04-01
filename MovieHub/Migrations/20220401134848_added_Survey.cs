using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MovieHub.Migrations
{
    public partial class added_Survey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Survey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CinemaNumber = table.Column<int>(type: "integer", nullable: false),
                    TicketPrice = table.Column<int>(type: "integer", nullable: false),
                    ScreenQuality = table.Column<int>(type: "integer", nullable: false),
                    SoundQuality = table.Column<int>(type: "integer", nullable: false),
                    PopcornQuality = table.Column<int>(type: "integer", nullable: false),
                    Nuisance = table.Column<int>(type: "integer", nullable: false),
                    Hygiene = table.Column<int>(type: "integer", nullable: false),
                    ToiletHeight = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Survey", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Survey");
        }
    }
}
