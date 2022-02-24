using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MovieHub.Migrations
{
    public partial class CinemaTickets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelaxChairs",
                table: "Hall");

            migrationBuilder.CreateTable(
                name: "MovieGenre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GenreId = table.Column<int>(type: "integer", nullable: false),
                    MovieId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieGenre", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieGenre_Genre_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieGenre_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CinemaTicketType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CinemaId = table.Column<int>(type: "integer", nullable: false),
                    TicketTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaTicketType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CinemaTicketType_Cinema_CinemaId",
                        column: x => x.CinemaId,
                        principalTable: "Cinema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CinemaTicketType_TicketType_TicketTypeId",
                        column: x => x.TicketTypeId,
                        principalTable: "TicketType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CinemaTicketType_CinemaId",
                table: "CinemaTicketType",
                column: "CinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_CinemaTicketType_TicketTypeId",
                table: "CinemaTicketType",
                column: "TicketTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenre_GenreId",
                table: "MovieGenre",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenre_MovieId",
                table: "MovieGenre",
                column: "MovieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CinemaTicketType");

            migrationBuilder.DropTable(
                name: "MovieGenre");

            migrationBuilder.DropTable(
                name: "TicketType");

            migrationBuilder.AddColumn<bool>(
                name: "RelaxChairs",
                table: "Hall",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
