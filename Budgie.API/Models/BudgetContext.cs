using Microsoft.EntityFrameworkCore;
using BudgetApi.Enums;

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
        Category dining = new Category { Id = Guid.NewGuid().ToString(), Name = "Dining" };
        Category transport = new Category { Id = Guid.NewGuid().ToString(), Name = "Transport" };
        Category rent = new Category { Id = Guid.NewGuid().ToString(), Name = "Rent" };
        Category homeImprovement = new Category { Id = Guid.NewGuid().ToString(), Name = "Home Improvement" };
        Category entertainment = new Category { Id = Guid.NewGuid().ToString(), Name = "Entertainment" };
        Category shopping = new Category { Id = Guid.NewGuid().ToString(), Name = "Shopping" };
        Category travel = new Category { Id = Guid.NewGuid().ToString(), Name = "Travel" };
        Category fitness = new Category { Id = Guid.NewGuid().ToString(), Name = "Fitness" };
        Category healthcare = new Category { Id = Guid.NewGuid().ToString(), Name = "Healthcare" };
        Category transfer = new Category { Id = Guid.NewGuid().ToString(), Name = "Transfer" };
        Category income = new Category { Id = Guid.NewGuid().ToString(), Name = "Income" };

        modelBuilder.Entity<Category>().HasData(
            groceries,
            utilities,
            dining,
            transport,
            rent,
            homeImprovement,
            entertainment,
            shopping,
            travel,
            fitness,
            healthcare,
            transfer,
            income
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
                    Description = "Coles Supermarket Southbank",
                    Amount = 85.50m,
                    Type = TransactionType.Expense,
                    CategoryId = groceries.Id
                },
                new Transaction
                {
                    Id = 2,
                    UserId = mockUser.Id,
                    Date = DateOnly.Parse("2024-01-02"),
                    Description = "Electricity Bill - AGL",
                    Amount = 120.75m,
                    Type = TransactionType.Expense,
                    CategoryId = utilities.Id
                },
                new Transaction
                {
                    Id = 3,
                    UserId = mockUser.Id,
                    Date = DateOnly.Parse("2024-01-05"),
                    Description = "Starbucks Queen St",
                    Amount = 6.20m,
                    Type = TransactionType.Expense,
                    CategoryId = dining.Id
                },
                new Transaction
                {
                    Id = 4,
                    UserId = mockUser.Id,
                    Date = DateOnly.Parse("2024-01-07"),
                    Description = "Myki Top-Up",
                    Amount = 50.00m,
                    Type = TransactionType.Expense,
                    CategoryId = transport.Id
                },
                new Transaction
                {
                    Id = 5,
                    UserId = mockUser.Id,
                    Date = DateOnly.Parse("2024-01-10"),
                    Description = "Rent - 123 Main St",
                    Amount = 1500.00m,
                    Type = TransactionType.Expense,
                    CategoryId = rent.Id
                },
                new Transaction
                {
                    Id = 6,
                    UserId = mockUser.Id,
                    Date = DateOnly.Parse("2024-01-12"),
                    Description = "Bunnings Hardware Capalaba",
                    Amount = 45.00m,
                    Type = TransactionType.Expense,
                    CategoryId = homeImprovement.Id
                },
                new Transaction
                {
                    Id = 7,
                    UserId = mockUser.Id,
                    Date = DateOnly.Parse("2024-01-15"),
                    Description = "Netflix Subscription",
                    Amount = 19.99m,
                    Type = TransactionType.Expense,
                    CategoryId = entertainment.Id
                },
                new Transaction
                {
                    Id = 8,
                    UserId = mockUser.Id,
                    Date = DateOnly.Parse("2024-01-18"),
                    Description = "Optus Bill",
                    Amount = 60.00m,
                    Type = TransactionType.Expense,
                    CategoryId = utilities.Id
                },
                new Transaction
                {
                    Id = 9,
                    UserId = mockUser.Id,
                    Date = DateOnly.Parse("2024-01-20"),
                    Description = "BP Fuel Cannon Hill",
                    Amount = 75.30m,
                    Type = TransactionType.Expense,
                    CategoryId = transport.Id
                },
                new Transaction
                {
                    Id = 10,
                    UserId = mockUser.Id,
                    Date = DateOnly.Parse("2024-01-22"),
                    Description = "Kmart Purchase Carindale",
                    Amount = 25.50m,
                    Type = TransactionType.Expense,
                    CategoryId = shopping.Id
                },
                new Transaction
                {
                    Id = 11,
                    UserId = mockUser.Id,
                    Date = DateOnly.Parse("2024-01-25"),
                    Description = "Qantas Flight QF123",
                    Amount = 450.00m,
                    Type = TransactionType.Expense,
                    CategoryId = travel.Id
                },
                new Transaction
                {
                    Id = 12,
                    UserId = mockUser.Id,
                    Date = DateOnly.Parse("2024-01-27"),
                    Description = "Woolworths Grocery Morningside",
                    Amount = 92.45m,
                    Type = TransactionType.Expense,
                    CategoryId = groceries.Id
                },
                new Transaction
                {
                    Id = 13,
                    UserId = mockUser.Id,
                    Date = DateOnly.Parse("2024-01-30"),
                    Description = "Goodlife Gym Membership",
                    Amount = 40.00m,
                    Type = TransactionType.Expense,
                    CategoryId = fitness.Id
                },
                new Transaction
                {
                    Id = 14,
                    UserId = mockUser.Id,
                    Date = DateOnly.Parse("2024-02-01"),
                    Description = "Uber Eats Order",
                    Amount = 32.80m,
                    Type = TransactionType.Expense,
                    CategoryId = dining.Id
                },
                new Transaction
                {
                    Id = 15,
                    UserId = mockUser.Id,
                    Date = DateOnly.Parse("2024-02-03"),
                    Description = "Spotify Subscription",
                    Amount = 11.99m,
                    Type = TransactionType.Expense,
                    CategoryId = entertainment.Id
                },
                new Transaction
                {
                    Id = 16,
                    UserId = mockUser.Id,
                    Date = DateOnly.Parse("2024-02-05"),
                    Description = "Medicare Bulk Billing",
                    Amount = 30.00m,
                    Type = TransactionType.Expense,
                    CategoryId = healthcare.Id
                },
                new Transaction
                {
                    Id = 17,
                    UserId = mockUser.Id,
                    Date = DateOnly.Parse("2024-02-07"),
                    Description = "JB Hi-Fi Electronics Purchase",
                    Amount = 150.00m,
                    Type = TransactionType.Expense,
                    CategoryId = shopping.Id
                },
                new Transaction
                {
                    Id = 18,
                    UserId = mockUser.Id,
                    Date = DateOnly.Parse("2024-02-08"),
                    Description = "Salary Deposit",
                    Amount = 3000.00m,
                    Type = TransactionType.Income,
                    CategoryId = income.Id
                },
                new Transaction
                {
                    Id = 19,
                    UserId = mockUser.Id,
                    Date = DateOnly.Parse("2024-02-10"),
                    Description = "Savings Transfer",
                    Amount = 500.00m,
                    Type = TransactionType.Transfer,
                    CategoryId = transfer.Id
                }
            );
        }
    }
}