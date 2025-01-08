using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalAccounting.Domain.Migrations
{
    /// <inheritdoc />
    public partial class receiptShopName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Receipts",
                newName: "ShopName");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "ReceiptItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "ReceiptItems");

            migrationBuilder.RenameColumn(
                name: "ShopName",
                table: "Receipts",
                newName: "Category");
        }
    }
}
