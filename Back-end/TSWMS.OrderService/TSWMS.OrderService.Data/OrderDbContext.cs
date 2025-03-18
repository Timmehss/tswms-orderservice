#region Usings

using Microsoft.EntityFrameworkCore;
using TSWMS.OrderService.Shared.Models;

#endregion

namespace TSWMS.OrderService.Data;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrderItem>()
            .HasKey(orderItem => new { orderItem.OrderId, orderItem.ProductId });

        modelBuilder.Entity<Order>()
        .Property(o => o.TotalAmount).HasColumnType("decimal(18,4)");

        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.Price)
            .HasColumnType("decimal(18,4)");

        // Seed data for Orders
        modelBuilder.Entity<Order>().HasData(
            new Order
            {
                OrderId = Guid.Parse("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"),
                UserId = Guid.Parse("52348777-7a0e-4139-9489-87dff9d47b7e"),
                TotalAmount = 160.00m,
                OrderDate = new DateTime(2024, 7, 15)
            },
            new Order
            {
                OrderId = Guid.Parse("fd92009e-9c89-45f8-9ac6-29edeeefce61"),
                UserId = Guid.Parse("7ee4caea-21e9-4261-947d-8305df18ff45"),
                TotalAmount = 125.00m,
                OrderDate = new DateTime(2024, 7, 16)
            },
            new Order
            {
                OrderId = Guid.Parse("beca8a32-5477-4b27-80f7-3495936edfd8"),
                UserId = Guid.Parse("27d0bb84-4420-441c-a503-264f1e365c05"),
                TotalAmount = 210.00m,
                OrderDate = new DateTime(2024, 7, 17)
            }
        );

        // Seed data for OrderItems
        modelBuilder.Entity<OrderItem>().HasData(
            new
            {
                OrderId = Guid.Parse("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"),
                ProductId = Guid.Parse("de3c9457-f6a8-4b4b-a4c1-f9d3db6f1e1d"),
                Quantity = 1,
                Price = 30.00m
            },
            new
            {
                OrderId = Guid.Parse("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"),
                ProductId = Guid.Parse("e8794f64-d634-4b74-a0a2-35f4fbf7b486"),
                Quantity = 1,
                Price = 40.00m
            },
            new
            {
                OrderId = Guid.Parse("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"),
                ProductId = Guid.Parse("ff465d9f-4060-44a3-a5ec-5aeb53f8c810"),
                Quantity = 1,
                Price = 50.00m
            },
            new
            {
                OrderId = Guid.Parse("a3c99b75-b0a5-4a3b-9c8c-34eed285f269"),
                ProductId = Guid.Parse("7adf9a42-b9fd-47f2-bf89-a09153ce7ab8"),
                Quantity = 1,
                Price = 40.00m
            },
            new
            {
                OrderId = Guid.Parse("fd92009e-9c89-45f8-9ac6-29edeeefce61"),
                ProductId = Guid.Parse("4d76bc3a-168d-4054-8154-6f6246355e53"),
                Quantity = 1,
                Price = 25.00m
            },
            new
            {
                OrderId = Guid.Parse("fd92009e-9c89-45f8-9ac6-29edeeefce61"),
                ProductId = Guid.Parse("bcc9673c-5c3f-4798-808f-b48e372b1dae"),
                Quantity = 1,
                Price = 30.00m
            },
            new
            {
                OrderId = Guid.Parse("fd92009e-9c89-45f8-9ac6-29edeeefce61"),
                ProductId = Guid.Parse("15d94488-db9b-4767-9e92-d6328b735e0e"),
                Quantity = 1,
                Price = 30.00m
            },
            new
            {
                OrderId = Guid.Parse("fd92009e-9c89-45f8-9ac6-29edeeefce61"),
                ProductId = Guid.Parse("9e5d721f-f729-4e8f-8ebe-3704d476fbeb"),
                Quantity = 1,
                Price = 40.00m
            },
            new
            {
                OrderId = Guid.Parse("beca8a32-5477-4b27-80f7-3495936edfd8"),
                ProductId = Guid.Parse("0e5d9b8b-c30d-4d53-8298-a39e0acadcfc"),
                Quantity = 1,
                Price = 50.00m
            },
            new
            {
                OrderId = Guid.Parse("beca8a32-5477-4b27-80f7-3495936edfd8"),
                ProductId = Guid.Parse("7b13656f-8a1f-4ba4-9dfb-340f2e1c362c"),
                Quantity = 1,
                Price = 55.00m
            },
            new
            {
                OrderId = Guid.Parse("beca8a32-5477-4b27-80f7-3495936edfd8"),
                ProductId = Guid.Parse("6609f9d5-1b3e-4c7e-bccc-996c7bda68c8"),
                Quantity = 1,
                Price = 55.00m
            },
            new
            {
                OrderId = Guid.Parse("beca8a32-5477-4b27-80f7-3495936edfd8"),
                ProductId = Guid.Parse("a45d88c6-5d79-4c8b-9fc0-b0bb3eb3ab88"),
                Quantity = 1,
                Price = 50.00m
            }
        );
    }
}
