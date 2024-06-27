using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalAccounting.Domain
{
    /// <inheritdoc />
    public partial class lastEditDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditDate",
                table: "TransferRequests",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastEditDate",
                table: "TransferRequests");
        }
    }
}
