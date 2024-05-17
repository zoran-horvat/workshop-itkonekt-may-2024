using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.Migrations
{
    /// <inheritdoc />
    public partial class AddedBookPriceToBookForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BookPrices_BookId",
                schema: "Books",
                table: "BookPrices",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookPrices_Books_BookId",
                schema: "Books",
                table: "BookPrices",
                column: "BookId",
                principalSchema: "Books",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookPrices_Books_BookId",
                schema: "Books",
                table: "BookPrices");

            migrationBuilder.DropIndex(
                name: "IX_BookPrices_BookId",
                schema: "Books",
                table: "BookPrices");
        }
    }
}
