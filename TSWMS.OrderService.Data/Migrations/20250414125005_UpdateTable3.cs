using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSWMS.OrderService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"), new Guid("ff465d9f-4060-44a3-a5ec-5aeb53f8c810") },
                columns: new[] { "Quantity", "TotalPrice" },
                values: new object[] { 2, 100.00m });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("fd92009e-9c89-45f8-9ac6-29edeeefce61"), new Guid("4d76bc3a-168d-4054-8154-6f6246355e53") },
                columns: new[] { "Quantity", "TotalPrice" },
                values: new object[] { 3, 75.00m });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("fd92009e-9c89-45f8-9ac6-29edeeefce61"), new Guid("9e5d721f-f729-4e8f-8ebe-3704d476fbeb") },
                columns: new[] { "Quantity", "TotalPrice" },
                values: new object[] { 5, 200.00m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "OrderItems",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"), new Guid("ff465d9f-4060-44a3-a5ec-5aeb53f8c810") },
                columns: new[] { "Quantity", "TotalPrice" },
                values: new object[] { 1, 50.00m });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("fd92009e-9c89-45f8-9ac6-29edeeefce61"), new Guid("4d76bc3a-168d-4054-8154-6f6246355e53") },
                columns: new[] { "Quantity", "TotalPrice" },
                values: new object[] { 1, 25.00m });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("fd92009e-9c89-45f8-9ac6-29edeeefce61"), new Guid("9e5d721f-f729-4e8f-8ebe-3704d476fbeb") },
                columns: new[] { "Quantity", "TotalPrice" },
                values: new object[] { 1, 40.00m });
        }
    }
}
