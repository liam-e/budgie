using Microsoft.EntityFrameworkCore;

namespace BudgetApi.Models;

public class BudgetContext : DbContext
{
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=127.0.0.1;Username=postgres;Password=insecure_password;Database=budget");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Categories
        Category groceries = new Category { Id = Guid.NewGuid().ToString(), Name = "Groceries" };
        Category utilities = new Category { Id = Guid.NewGuid().ToString(), Name = "Utilities" };

        modelBuilder.Entity<Category>().HasData(
            groceries,
            utilities
        );

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            // Users
            User mockUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = "user@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password1")
            };

            modelBuilder.Entity<User>().HasData(
                mockUser
            );

            // Transactions
            modelBuilder.Entity<Transaction>().HasData(
                new Transaction
                {
                    Id = 1,
                    UserId = mockUser.Id,
                    Date = DateOnly.Parse("2024-01-01"),
                    Description = "Supermarket",
                    Amount = 20,
                    CategoryId = groceries.Id
                },
                new Transaction
                {
                    Id = 2,
                    UserId = mockUser.Id,
                    Date = DateOnly.Parse("2024-01-02"),
                    Description = "Electricity Bill",
                    Amount = 100,
                    CategoryId = utilities.Id
                }
            );
        }
    }
}