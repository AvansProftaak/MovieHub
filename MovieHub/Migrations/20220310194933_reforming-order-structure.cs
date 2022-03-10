using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieHub.Migrations
{
    public partial class reformingorderstructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_CateringPackage_CateringPackageId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Tickettype_TickettypeId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_TickettypeId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Order_CateringPackageId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "TickettypeId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "CateringPackageId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PaidAt",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Order");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Ticket",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "Order",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Ticket");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Ticket",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TickettypeId",
                table: "Ticket",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "Payment",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "TotalPrice",
                table: "Order",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<int>(
                name: "CateringPackageId",
                table: "Order",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidAt",
                table: "Order",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Order",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TickettypeId",
                table: "Ticket",
                column: "TickettypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CateringPackageId",
                table: "Order",
                column: "CateringPackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_CateringPackage_CateringPackageId",
                table: "Order",
                column: "CateringPackageId",
                principalTable: "CateringPackage",
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
    }
}
