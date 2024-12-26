using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalAccounting.Domain.Migrations
{
    /// <inheritdoc />
    public partial class transfer_last_edit_fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastEditUserFullName",
                table: "TransferRequests",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastEditUserName",
                table: "TransferRequests",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastEditUserFullName",
                table: "TransferRequests");

            migrationBuilder.DropColumn(
                name: "LastEditUserName",
                table: "TransferRequests");
        }
    }
}
