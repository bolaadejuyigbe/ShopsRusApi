using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopsRUsAPi.Migrations
{
    public partial class CreateInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    customerId = table.Column<long>(type: "INTEGER", nullable: false),
                    discountId = table.Column<long>(type: "INTEGER", nullable: false),
                    billDiscountPercentage = table.Column<decimal>(type: "TEXT", nullable: false),
                    totalDiscountAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    totalproductAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    totalAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Customers_customerId",
                        column: x => x.customerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_Discounts_discountId",
                        column: x => x.discountId,
                        principalTable: "Discounts",
                        principalColumn: "DiscountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    itemAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    productId = table.Column<long>(type: "INTEGER", nullable: false),
                    invoicesId = table.Column<long>(type: "INTEGER", nullable: false),
                    isGrocery = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Invoices_invoicesId",
                        column: x => x.invoicesId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_customerId",
                table: "Invoices",
                column: "customerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_discountId",
                table: "Invoices",
                column: "discountId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_invoicesId",
                table: "Items",
                column: "invoicesId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_productId",
                table: "Items",
                column: "productId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Invoices");
        }
    }
}
