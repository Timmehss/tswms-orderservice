using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TSWMS.UserService.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitUserDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "Password" },
                values: new object[,]
                {
                    { new Guid("27d0bb84-4420-441c-a503-264f1e365c05"), "user3@example.com", "password3" },
                    { new Guid("52348777-7a0e-4139-9489-87dff9d47b7e"), "user1@example.com", "password1" },
                    { new Guid("7ee4caea-21e9-4261-947d-8305df18ff45"), "user2@example.com", "password2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
