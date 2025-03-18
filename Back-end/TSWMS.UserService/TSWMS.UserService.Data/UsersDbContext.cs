using Microsoft.EntityFrameworkCore;
using TSWMS.UserService.Shared.Models;

namespace TSWMS.UserService.Data;

public class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data for Users
        modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId = Guid.Parse("52348777-7a0e-4139-9489-87dff9d47b7e"),
                Email = "user1@example.com",
                Password = "password1"
            },
            new User
            {
                UserId = Guid.Parse("7ee4caea-21e9-4261-947d-8305df18ff45"),
                Email = "user2@example.com",
                Password = "password2"
            },
            new User
            {
                UserId = Guid.Parse("27d0bb84-4420-441c-a503-264f1e365c05"),
                Email = "user3@example.com",
                Password = "password3"
            }
        );
    }
}
