using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.Migrations
{
    /// <inheritdoc />
    public partial class AddedInvoiceDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Timestamp",
                schema: "Books",
                table: "Invoices",
                newName: "IssueTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                schema: "Books",
                table: "Invoices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentTime",
                schema: "Books",
                table: "Invoices",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDate",
                schema: "Books",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "PaymentTime",
                schema: "Books",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "IssueTime",
                schema: "Books",
                table: "Invoices",
                newName: "Timestamp");
        }
    }
}
