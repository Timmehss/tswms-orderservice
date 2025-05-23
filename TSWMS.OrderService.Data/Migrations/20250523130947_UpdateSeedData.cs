using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSWMS.OrderService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: new Guid("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"),
                column: "TotalPrice",
                value: 210.00m);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: new Guid("fd92009e-9c89-45f8-9ac6-29edeeefce61"),
                column: "TotalPrice",
                value: 335.00m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: new Guid("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"),
                column: "TotalPrice",
                value: 160.00m);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: new Guid("fd92009e-9c89-45f8-9ac6-29edeeefce61"),
                column: "TotalPrice",
                value: 125.00m);
        }
    }
}
