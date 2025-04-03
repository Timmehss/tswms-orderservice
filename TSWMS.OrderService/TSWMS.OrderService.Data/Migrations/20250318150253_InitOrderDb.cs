using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TSWMS.OrderService.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitOrderDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => new { x.OrderId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "OrderDate", "TotalAmount", "UserId" },
                values: new object[,]
                {
                    { new Guid("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"), new DateTime(2024, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 160.00m, new Guid("52348777-7a0e-4139-9489-87dff9d47b7e") },
                    { new Guid("beca8a32-5477-4b27-80f7-3495936edfd8"), new DateTime(2024, 7, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 210.00m, new Guid("27d0bb84-4420-441c-a503-264f1e365c05") },
                    { new Guid("fd92009e-9c89-45f8-9ac6-29edeeefce61"), new DateTime(2024, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 125.00m, new Guid("7ee4caea-21e9-4261-947d-8305df18ff45") }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "OrderId", "ProductId", "Price", "Quantity" },
                values: new object[,]
                {
                    { new Guid("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"), new Guid("7adf9a42-b9fd-47f2-bf89-a09153ce7ab8"), 40.00m, 1 },
                    { new Guid("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"), new Guid("de3c9457-f6a8-4b4b-a4c1-f9d3db6f1e1d"), 30.00m, 1 },
                    { new Guid("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"), new Guid("e8794f64-d634-4b74-a0a2-35f4fbf7b486"), 40.00m, 1 },
                    { new Guid("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"), new Guid("ff465d9f-4060-44a3-a5ec-5aeb53f8c810"), 50.00m, 1 },
                    { new Guid("beca8a32-5477-4b27-80f7-3495936edfd8"), new Guid("0e5d9b8b-c30d-4d53-8298-a39e0acadcfc"), 50.00m, 1 },
                    { new Guid("beca8a32-5477-4b27-80f7-3495936edfd8"), new Guid("6609f9d5-1b3e-4c7e-bccc-996c7bda68c8"), 55.00m, 1 },
                    { new Guid("beca8a32-5477-4b27-80f7-3495936edfd8"), new Guid("7b13656f-8a1f-4ba4-9dfb-340f2e1c362c"), 55.00m, 1 },
                    { new Guid("beca8a32-5477-4b27-80f7-3495936edfd8"), new Guid("a45d88c6-5d79-4c8b-9fc0-b0bb3eb3ab88"), 50.00m, 1 },
                    { new Guid("fd92009e-9c89-45f8-9ac6-29edeeefce61"), new Guid("15d94488-db9b-4767-9e92-d6328b735e0e"), 30.00m, 1 },
                    { new Guid("fd92009e-9c89-45f8-9ac6-29edeeefce61"), new Guid("4d76bc3a-168d-4054-8154-6f6246355e53"), 25.00m, 1 },
                    { new Guid("fd92009e-9c89-45f8-9ac6-29edeeefce61"), new Guid("9e5d721f-f729-4e8f-8ebe-3704d476fbeb"), 40.00m, 1 },
                    { new Guid("fd92009e-9c89-45f8-9ac6-29edeeefce61"), new Guid("bcc9673c-5c3f-4798-808f-b48e372b1dae"), 30.00m, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
