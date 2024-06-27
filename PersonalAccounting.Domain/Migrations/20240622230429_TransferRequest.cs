using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalAccounting.Domain
{
    /// <inheritdoc />
    public partial class TransferRequestMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TransferRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FromUserId = table.Column<string>(type: "TEXT", nullable: false),
                    FromUserName = table.Column<string>(type: "TEXT", nullable: false),
                    FromUserFullName = table.Column<string>(type: "TEXT", nullable: false),
                    ToUserId = table.Column<string>(type: "TEXT", nullable: false),
                    ToUserName = table.Column<string>(type: "TEXT", nullable: false),
                    ToUserFullName = table.Column<string>(type: "TEXT", nullable: false),
                    ReceiverUserId = table.Column<string>(type: "TEXT", nullable: false),
                    ReceiverUserName = table.Column<string>(type: "TEXT", nullable: false),
                    ReceiverUserFullName = table.Column<string>(type: "TEXT", nullable: false),
                    ReceiverAccountNo = table.Column<string>(type: "TEXT", nullable: false),
                    ReceiverCardNo = table.Column<string>(type: "TEXT", nullable: false),
                    ReceiverNote = table.Column<string>(type: "TEXT", nullable: false),
                    SourceCurrencyName = table.Column<string>(type: "TEXT", nullable: false),
                    SourceAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    DestinationCurrencyName = table.Column<string>(type: "TEXT", nullable: false),
                    DestinationAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    Fee = table.Column<decimal>(type: "TEXT", nullable: false),
                    MyProperty = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransferRequestDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TransferRequestId = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferRequestDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferRequestDetails_TransferRequests_TransferRequestId",
                        column: x => x.TransferRequestId,
                        principalTable: "TransferRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransferRequestDetails_TransferRequestId",
                table: "TransferRequestDetails",
                column: "TransferRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransferRequestDetails");

            migrationBuilder.DropTable(
                name: "TransferRequests");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "AspNetUsers");
        }
    }
}
