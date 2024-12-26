using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalAccounting.Domain.Migrations
{
    /// <inheritdoc />
    public partial class transfer_last_edit_user_id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastEditUserId",
                table: "TransferRequests",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastEditUserId",
                table: "TransferRequests");
        }
    }
}
