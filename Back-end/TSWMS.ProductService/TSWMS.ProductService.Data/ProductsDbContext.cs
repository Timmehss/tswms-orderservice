using Microsoft.EntityFrameworkCore;
using TSWMS.ProductService.Shared.Models;

namespace TSWMS.ProductService.Data;

public class ProductsDbContext : DbContext
{
    public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
    new Product
    {
        ProductId = Guid.Parse("de3c9457-f6a8-4b4b-a4c1-f9d3db6f1e1d"),
        Name = "HDMI Cable 1.5m",
        AvailableStock = 100
    },
    new Product
    {
        ProductId = Guid.Parse("e8794f64-d634-4b74-a0a2-35f4fbf7b486"),
        Name = "USB-C Cable 2m",
        AvailableStock = 150
    },
    new Product
    {
        ProductId = Guid.Parse("ff465d9f-4060-44a3-a5ec-5aeb53f8c810"),
        Name = "Lightning Cable 1m",
        AvailableStock = 200
    },
    new Product
    {
        ProductId = Guid.Parse("7adf9a42-b9fd-47f2-bf89-a09153ce7ab8"),
        Name = "HDMI Cable 3m",
        AvailableStock = 120
    },
    new Product
    {
        ProductId = Guid.Parse("4d76bc3a-168d-4054-8154-6f6246355e53"),
        Name = "USB-C Cable 1.5m",
        AvailableStock = 90
    },
    new Product
    {
        ProductId = Guid.Parse("bcc9673c-5c3f-4798-808f-b48e372b1dae"),
        Name = "Lightning Cable 2m",
        AvailableStock = 110
    },
    new Product
    {
        ProductId = Guid.Parse("15d94488-db9b-4767-9e92-d6328b735e0e"),
        Name = "HDMI Cable 5m",
        AvailableStock = 80
    },
    new Product
    {
        ProductId = Guid.Parse("9e5d721f-f729-4e8f-8ebe-3704d476fbeb"),
        Name = "USB-C Cable 0.5m",
        AvailableStock = 70
    },
    new Product
    {
        ProductId = Guid.Parse("0e5d9b8b-c30d-4d53-8298-a39e0acadcfc"),
        Name = "Lightning Cable 1.5m",
        AvailableStock = 75
    },
    new Product
    {
        ProductId = Guid.Parse("7b13656f-8a1f-4ba4-9dfb-340f2e1c362c"),
        Name = "HDMI Cable 10m",
        AvailableStock = 95
    },
    new Product
    {
        ProductId = Guid.Parse("6609f9d5-1b3e-4c7e-bccc-996c7bda68c8"),
        Name = "USB-C Cable 3m",
        AvailableStock = 60
    },
    new Product
    {
        ProductId = Guid.Parse("a45d88c6-5d79-4c8b-9fc0-b0bb3eb3ab88"),
        Name = "Lightning Cable 5m",
        AvailableStock = 85
    }
);
    }
}
