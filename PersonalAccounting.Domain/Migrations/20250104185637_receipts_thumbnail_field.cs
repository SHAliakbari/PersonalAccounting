using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalAccounting.Domain.Migrations
{
    /// <inheritdoc />
    public partial class receipts_thumbnail_field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PayedByUserName",
                table: "Receipts",
                newName: "PaidByUserName");

            migrationBuilder.RenameColumn(
                name: "PayedByUserId",
                table: "Receipts",
                newName: "PaidByUserId");

            migrationBuilder.RenameColumn(
                name: "PayedByUserFullName",
                table: "Receipts",
                newName: "PaidByUserFullName");

            migrationBuilder.AddColumn<byte[]>(
                name: "Thumbnail",
                table: "Receipts",
                type: "BLOB",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "Receipts");

            migrationBuilder.RenameColumn(
                name: "PaidByUserName",
                table: "Receipts",
                newName: "PayedByUserName");

            migrationBuilder.RenameColumn(
                name: "PaidByUserId",
                table: "Receipts",
                newName: "PayedByUserId");

            migrationBuilder.RenameColumn(
                name: "PaidByUserFullName",
                table: "Receipts",
                newName: "PayedByUserFullName");
        }
    }
}
