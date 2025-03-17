using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddStockHistories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPrices_ProductSizes_ProductSizeId",
                table: "ProductPrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPrices",
                table: "ProductPrices");

            migrationBuilder.RenameTable(
                name: "ProductPrices",
                newName: "ProductPrice");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPrices_ProductSizeId",
                table: "ProductPrice",
                newName: "IX_ProductPrice_ProductSizeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPrice",
                table: "ProductPrice",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPrice_ProductSizes_ProductSizeId",
                table: "ProductPrice",
                column: "ProductSizeId",
                principalTable: "ProductSizes",
                principalColumn: "ProductSizeID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPrice_ProductSizes_ProductSizeId",
                table: "ProductPrice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPrice",
                table: "ProductPrice");

            migrationBuilder.RenameTable(
                name: "ProductPrice",
                newName: "ProductPrices");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPrice_ProductSizeId",
                table: "ProductPrices",
                newName: "IX_ProductPrices_ProductSizeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPrices",
                table: "ProductPrices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPrices_ProductSizes_ProductSizeId",
                table: "ProductPrices",
                column: "ProductSizeId",
                principalTable: "ProductSizes",
                principalColumn: "ProductSizeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
