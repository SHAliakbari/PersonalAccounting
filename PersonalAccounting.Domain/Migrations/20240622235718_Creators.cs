using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalAccounting.Domain
{
    /// <inheritdoc />
    public partial class Creators : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "TransferRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserFullName",
                table: "TransferRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "TransferRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreateUserName",
                table: "TransferRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "TransferRequestDetails",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreateUserFullName",
                table: "TransferRequestDetails",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "TransferRequestDetails",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreateUserName",
                table: "TransferRequestDetails",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "TransferRequests");

            migrationBuilder.DropColumn(
                name: "CreateUserFullName",
                table: "TransferRequests");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "TransferRequests");

            migrationBuilder.DropColumn(
                name: "CreateUserName",
                table: "TransferRequests");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "TransferRequestDetails");

            migrationBuilder.DropColumn(
                name: "CreateUserFullName",
                table: "TransferRequestDetails");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "TransferRequestDetails");

            migrationBuilder.DropColumn(
                name: "CreateUserName",
                table: "TransferRequestDetails");
        }
    }
}
