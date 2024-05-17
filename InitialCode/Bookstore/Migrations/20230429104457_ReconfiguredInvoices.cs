using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.Migrations
{
    /// <inheritdoc />
    public partial class ReconfiguredInvoices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Customers_CustomerId",
                schema: "Books",
                table: "Invoice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoice",
                schema: "Books",
                table: "Invoice");

            migrationBuilder.RenameTable(
                name: "Invoice",
                schema: "Books",
                newName: "Invoices",
                newSchema: "Books");

            migrationBuilder.RenameIndex(
                name: "IX_Invoice_CustomerId",
                schema: "Books",
                table: "Invoices",
                newName: "IX_Invoices_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoices",
                schema: "Books",
                table: "Invoices",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "InvoiceLines",
                schema: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceLines_Books_BookId",
                        column: x => x.BookId,
                        principalSchema: "Books",
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceLines_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "Books",
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_BookId",
                schema: "Books",
                table: "InvoiceLines",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_InvoiceId",
                schema: "Books",
                table: "InvoiceLines",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Customers_CustomerId",
                schema: "Books",
                table: "Invoices",
                column: "CustomerId",
                principalSchema: "Books",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Customers_CustomerId",
                schema: "Books",
                table: "Invoices");

            migrationBuilder.DropTable(
                name: "InvoiceLines",
                schema: "Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoices",
                schema: "Books",
                table: "Invoices");

            migrationBuilder.RenameTable(
                name: "Invoices",
                schema: "Books",
                newName: "Invoice",
                newSchema: "Books");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_CustomerId",
                schema: "Books",
                table: "Invoice",
                newName: "IX_Invoice_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoice",
                schema: "Books",
                table: "Invoice",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Customers_CustomerId",
                schema: "Books",
                table: "Invoice",
                column: "CustomerId",
                principalSchema: "Books",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
