using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStockHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockHistories_Products_ProductID",
                table: "StockHistories");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "StockHistories",
                newName: "ProductSizeID");

            migrationBuilder.RenameIndex(
                name: "IX_StockHistories_ProductID",
                table: "StockHistories",
                newName: "IX_StockHistories_ProductSizeID");

            migrationBuilder.AddForeignKey(
                name: "FK_StockHistories_ProductSizes_ProductSizeID",
                table: "StockHistories",
                column: "ProductSizeID",
                principalTable: "ProductSizes",
                principalColumn: "ProductSizeID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockHistories_ProductSizes_ProductSizeID",
                table: "StockHistories");

            migrationBuilder.RenameColumn(
                name: "ProductSizeID",
                table: "StockHistories",
                newName: "ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_StockHistories_ProductSizeID",
                table: "StockHistories",
                newName: "IX_StockHistories_ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_StockHistories_Products_ProductID",
                table: "StockHistories",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
