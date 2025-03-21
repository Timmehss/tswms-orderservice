using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TSWMS.ProductService.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitProductDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvailableStock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "AvailableStock", "Name" },
                values: new object[,]
                {
                    { new Guid("0e5d9b8b-c30d-4d53-8298-a39e0acadcfc"), 75, "Lightning Cable 1.5m" },
                    { new Guid("15d94488-db9b-4767-9e92-d6328b735e0e"), 80, "HDMI Cable 5m" },
                    { new Guid("4d76bc3a-168d-4054-8154-6f6246355e53"), 90, "USB-C Cable 1.5m" },
                    { new Guid("6609f9d5-1b3e-4c7e-bccc-996c7bda68c8"), 60, "USB-C Cable 3m" },
                    { new Guid("7adf9a42-b9fd-47f2-bf89-a09153ce7ab8"), 120, "HDMI Cable 3m" },
                    { new Guid("7b13656f-8a1f-4ba4-9dfb-340f2e1c362c"), 95, "HDMI Cable 10m" },
                    { new Guid("9e5d721f-f729-4e8f-8ebe-3704d476fbeb"), 70, "USB-C Cable 0.5m" },
                    { new Guid("a45d88c6-5d79-4c8b-9fc0-b0bb3eb3ab88"), 85, "Lightning Cable 5m" },
                    { new Guid("bcc9673c-5c3f-4798-808f-b48e372b1dae"), 110, "Lightning Cable 2m" },
                    { new Guid("de3c9457-f6a8-4b4b-a4c1-f9d3db6f1e1d"), 100, "HDMI Cable 1.5m" },
                    { new Guid("e8794f64-d634-4b74-a0a2-35f4fbf7b486"), 150, "USB-C Cable 2m" },
                    { new Guid("ff465d9f-4060-44a3-a5ec-5aeb53f8c810"), 200, "Lightning Cable 1m" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
