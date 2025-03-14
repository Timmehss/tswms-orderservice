using Microsoft.EntityFrameworkCore;
using UserService.Shared.Models;

namespace UserService.Data;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data for Users
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = new Guid("52348777-7a0e-4139-9489-87dff9d47b7e"),
                Email = "user1@example.com",
                Password = "password1"
            },
            new User
            {
                Id = new Guid("7ee4caea-21e9-4261-947d-8305df18ff45"),
                Email = "user2@example.com",
                Password = "password2"
            },
            new User
            {
                Id = new Guid("27d0bb84-4420-441c-a503-264f1e365c05"),
                Email = "user3@example.com",
                Password = "password3"
            }
        );
    }
}
