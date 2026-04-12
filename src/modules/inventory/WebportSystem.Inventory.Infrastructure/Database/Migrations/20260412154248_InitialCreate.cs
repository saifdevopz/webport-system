using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebportSystem.Inventory.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "inventory");

            migrationBuilder.CreateTable(
                name: "businessProfiles",
                schema: "inventory",
                columns: table => new
                {
                    businessProfileId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    businessName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    addressLine1 = table.Column<string>(type: "text", nullable: false),
                    postalCode = table.Column<string>(type: "text", nullable: false),
                    city = table.Column<string>(type: "text", nullable: false),
                    province = table.Column<string>(type: "text", nullable: false),
                    country = table.Column<string>(type: "text", nullable: false),
                    bankName = table.Column<string>(type: "text", nullable: true),
                    branchCode = table.Column<string>(type: "text", nullable: true),
                    accountNumber = table.Column<string>(type: "text", nullable: true),
                    logoUrl = table.Column<string>(type: "text", nullable: true),
                    isActive = table.Column<bool>(type: "boolean", nullable: false),
                    lastModBy = table.Column<string>(type: "text", nullable: false),
                    lastModDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdBy = table.Column<string>(type: "text", nullable: false),
                    createdDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_businessProfiles", x => x.businessProfileId);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "inventory",
                columns: table => new
                {
                    categoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    categoryCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    categoryDesc = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    isActive = table.Column<bool>(type: "boolean", nullable: false),
                    lastModBy = table.Column<string>(type: "text", nullable: false),
                    lastModDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdBy = table.Column<string>(type: "text", nullable: false),
                    createdDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_categories", x => x.categoryId);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                schema: "inventory",
                columns: table => new
                {
                    customerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    companyName = table.Column<string>(type: "text", nullable: false),
                    addressLine1 = table.Column<string>(type: "text", nullable: false),
                    postalCode = table.Column<string>(type: "text", nullable: false),
                    city = table.Column<string>(type: "text", nullable: false),
                    province = table.Column<string>(type: "text", nullable: false),
                    isActive = table.Column<bool>(type: "boolean", nullable: false),
                    lastModBy = table.Column<string>(type: "text", nullable: false),
                    lastModDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdBy = table.Column<string>(type: "text", nullable: false),
                    createdDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_customers", x => x.customerId);
                });

            migrationBuilder.CreateTable(
                name: "outbox_message_consumers",
                schema: "inventory",
                columns: table => new
                {
                    outboxMessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_outbox_message_consumers", x => new { x.outboxMessageId, x.name });
                });

            migrationBuilder.CreateTable(
                name: "outbox_messages",
                schema: "inventory",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "jsonb", maxLength: 5000, nullable: false),
                    occurredOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_outbox_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "items",
                schema: "inventory",
                columns: table => new
                {
                    itemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    categoryId = table.Column<int>(type: "integer", nullable: false),
                    itemCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    itemDesc = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    sellingPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    costPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    isActive = table.Column<bool>(type: "boolean", nullable: false),
                    lastModBy = table.Column<string>(type: "text", nullable: false),
                    lastModDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdBy = table.Column<string>(type: "text", nullable: false),
                    createdDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_items", x => x.itemId);
                    table.ForeignKey(
                        name: "fK_items_categories_categoryId",
                        column: x => x.categoryId,
                        principalSchema: "inventory",
                        principalTable: "categories",
                        principalColumn: "categoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                schema: "inventory",
                columns: table => new
                {
                    invoiceId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    invoiceNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    businessProfileId = table.Column<int>(type: "integer", nullable: false),
                    customerId = table.Column<int>(type: "integer", nullable: true),
                    subTotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    total = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    isActive = table.Column<bool>(type: "boolean", nullable: false),
                    lastModBy = table.Column<string>(type: "text", nullable: false),
                    lastModDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdBy = table.Column<string>(type: "text", nullable: false),
                    createdDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_Invoices", x => x.invoiceId);
                    table.ForeignKey(
                        name: "fK_Invoices_businessProfiles_businessProfileId",
                        column: x => x.businessProfileId,
                        principalSchema: "inventory",
                        principalTable: "businessProfiles",
                        principalColumn: "businessProfileId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fK_Invoices_customers_customerId",
                        column: x => x.customerId,
                        principalSchema: "inventory",
                        principalTable: "customers",
                        principalColumn: "customerId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                schema: "inventory",
                columns: table => new
                {
                    invoiceItemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    invoiceId = table.Column<int>(type: "integer", nullable: false),
                    itemId = table.Column<int>(type: "integer", nullable: false),
                    itemName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    unitPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    total = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_InvoiceItems", x => x.invoiceItemId);
                    table.ForeignKey(
                        name: "fK_InvoiceItems_Invoices_invoiceId",
                        column: x => x.invoiceId,
                        principalSchema: "inventory",
                        principalTable: "Invoices",
                        principalColumn: "invoiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "iX_categories_categoryCode",
                schema: "inventory",
                table: "categories",
                column: "categoryCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "iX_customers_name",
                schema: "inventory",
                table: "customers",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "iX_InvoiceItems_invoiceId",
                schema: "inventory",
                table: "InvoiceItems",
                column: "invoiceId");

            migrationBuilder.CreateIndex(
                name: "iX_Invoices_businessProfileId",
                schema: "inventory",
                table: "Invoices",
                column: "businessProfileId");

            migrationBuilder.CreateIndex(
                name: "iX_Invoices_customerId",
                schema: "inventory",
                table: "Invoices",
                column: "customerId");

            migrationBuilder.CreateIndex(
                name: "iX_Invoices_invoiceNumber",
                schema: "inventory",
                table: "Invoices",
                column: "invoiceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "iX_items_categoryId",
                schema: "inventory",
                table: "items",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "iX_items_itemCode",
                schema: "inventory",
                table: "items",
                column: "itemCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceItems",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "items",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "outbox_message_consumers",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "outbox_messages",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "Invoices",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "categories",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "businessProfiles",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "customers",
                schema: "inventory");
        }
    }
}
