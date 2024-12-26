using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalAccounting.Domain.Migrations
{
    /// <inheritdoc />
    public partial class thumbnail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileContent",
                table: "TransferRequestDetails",
                newName: "Thumbnail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Thumbnail",
                table: "TransferRequestDetails",
                newName: "FileContent");
        }
    }
}
