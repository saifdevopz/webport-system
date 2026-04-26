using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebportSystem.Inventory.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "due_date",
                schema: "inventory",
                table: "invoices",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "invoice_date",
                schema: "inventory",
                table: "invoices",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "due_date",
                schema: "inventory",
                table: "invoices");

            migrationBuilder.DropColumn(
                name: "invoice_date",
                schema: "inventory",
                table: "invoices");
        }
    }
}
