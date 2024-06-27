using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalAccounting.Domain
{
    /// <inheritdoc />
    public partial class feeCurrencyName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FeeCurrencyName",
                table: "TransferRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeeCurrencyName",
                table: "TransferRequests");
        }
    }
}
