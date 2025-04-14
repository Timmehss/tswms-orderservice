using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSWMS.OrderService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "OrderItems",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"), new Guid("7adf9a42-b9fd-47f2-bf89-a09153ce7ab8") },
                column: "TotalPrice",
                value: 40.00m);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"), new Guid("de3c9457-f6a8-4b4b-a4c1-f9d3db6f1e1d") },
                column: "TotalPrice",
                value: 30.00m);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"), new Guid("e8794f64-d634-4b74-a0a2-35f4fbf7b486") },
                column: "TotalPrice",
                value: 40.00m);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"), new Guid("ff465d9f-4060-44a3-a5ec-5aeb53f8c810") },
                column: "TotalPrice",
                value: 50.00m);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("beca8a32-5477-4b27-80f7-3495936edfd8"), new Guid("0e5d9b8b-c30d-4d53-8298-a39e0acadcfc") },
                column: "TotalPrice",
                value: 50.00m);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("beca8a32-5477-4b27-80f7-3495936edfd8"), new Guid("6609f9d5-1b3e-4c7e-bccc-996c7bda68c8") },
                column: "TotalPrice",
                value: 55.00m);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("beca8a32-5477-4b27-80f7-3495936edfd8"), new Guid("7b13656f-8a1f-4ba4-9dfb-340f2e1c362c") },
                column: "TotalPrice",
                value: 55.00m);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("beca8a32-5477-4b27-80f7-3495936edfd8"), new Guid("a45d88c6-5d79-4c8b-9fc0-b0bb3eb3ab88") },
                column: "TotalPrice",
                value: 50.00m);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("fd92009e-9c89-45f8-9ac6-29edeeefce61"), new Guid("15d94488-db9b-4767-9e92-d6328b735e0e") },
                column: "TotalPrice",
                value: 30.00m);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("fd92009e-9c89-45f8-9ac6-29edeeefce61"), new Guid("4d76bc3a-168d-4054-8154-6f6246355e53") },
                column: "TotalPrice",
                value: 25.00m);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("fd92009e-9c89-45f8-9ac6-29edeeefce61"), new Guid("9e5d721f-f729-4e8f-8ebe-3704d476fbeb") },
                column: "TotalPrice",
                value: 40.00m);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("fd92009e-9c89-45f8-9ac6-29edeeefce61"), new Guid("bcc9673c-5c3f-4798-808f-b48e372b1dae") },
                column: "TotalPrice",
                value: 30.00m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "OrderItems");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "Orders",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
