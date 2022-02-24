using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieHub.Migrations
{
    public partial class Fkv3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Ticket_OrderId",
                table: "Ticket",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_SeatId",
                table: "Ticket",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TickettypeId",
                table: "Ticket",
                column: "TickettypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Showtime_HallId",
                table: "Showtime",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_Showtime_MovieId",
                table: "Showtime",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_OrderId",
                table: "Payment",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PaymentMethodId",
                table: "Payment",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Order_OrderId",
                table: "Payment",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_PaymentMethod_PaymentMethodId",
                table: "Payment",
                column: "PaymentMethodId",
                principalTable: "PaymentMethod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Showtime_Hall_HallId",
                table: "Showtime",
                column: "HallId",
                principalTable: "Hall",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Showtime_Movie_MovieId",
                table: "Showtime",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Order_OrderId",
                table: "Ticket",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Seat_SeatId",
                table: "Ticket",
                column: "SeatId",
                principalTable: "Seat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Tickettype_TickettypeId",
                table: "Ticket",
                column: "TickettypeId",
                principalTable: "Tickettype",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Order_OrderId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_PaymentMethod_PaymentMethodId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Showtime_Hall_HallId",
                table: "Showtime");

            migrationBuilder.DropForeignKey(
                name: "FK_Showtime_Movie_MovieId",
                table: "Showtime");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Order_OrderId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Seat_SeatId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Tickettype_TickettypeId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_OrderId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_SeatId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_TickettypeId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Showtime_HallId",
                table: "Showtime");

            migrationBuilder.DropIndex(
                name: "IX_Showtime_MovieId",
                table: "Showtime");

            migrationBuilder.DropIndex(
                name: "IX_Payment_OrderId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_PaymentMethodId",
                table: "Payment");
        }
    }
}
