using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;

namespace Budgie.API.Models;

public class BudgetContext : DbContext
{
    public required DbSet<Transaction> Transactions { get; set; }
    public required DbSet<User> Users { get; set; }
    public required DbSet<Category> Categories { get; set; }
    public required DbSet<TransactionType> TransactionTypes { get; set; }
    public required DbSet<BudgetLimit> BudgetLimits { get; set; } // Add this line

    private readonly string _dataFilePath;
    private readonly bool _isTestEnvironment;

    // Constructor now accepts DbContextOptions and DbConnection
    public BudgetContext(DbContextOptions<BudgetContext> options, DbConnection connection, IWebHostEnvironment env)
        : base(options)
    {
        _dataFilePath = Path.Combine(env.ContentRootPath, "Data");
        _isTestEnvironment = env.EnvironmentName == "Test";

        // Set the connection explicitly
        this.Database.SetDbConnection(connection);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connection = Database.GetDbConnection();
            optionsBuilder.UseNpgsql(connection);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique(); // Make the Email field unique

        modelBuilder.Entity<Category>()
            .HasOne(c => c.TransactionType)
            .WithMany(t => t.Categories)
            .HasForeignKey(c => c.TransactionTypeId);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Category)
        .WithMany()
        .HasForeignKey(t => t.CategoryId);

        // Add a composite unique index for transactions to prevent duplicates
        modelBuilder.Entity<Transaction>()
            .HasIndex(t => new { t.UserId, t.Date, t.OriginalDescription, t.Amount })
            .IsUnique();

        modelBuilder.Entity<BudgetLimit>()
            .HasOne(bl => bl.User)
            .WithMany()
            .HasForeignKey(bl => bl.UserId);

        modelBuilder.Entity<BudgetLimit>()
            .HasOne(bl => bl.Category)
            .WithMany()
            .HasForeignKey(bl => bl.CategoryId);

        modelBuilder.Entity<BudgetLimit>()
            .HasIndex(bl => new { bl.UserId, bl.CategoryId, bl.PeriodType })
            .IsUnique();

        modelBuilder.Entity<BudgetLimit>()
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<TransactionType>().HasKey(t => t.Id);
        modelBuilder.Entity<Category>().HasKey(c => c.Id);
        modelBuilder.Entity<Transaction>().HasKey(t => t.Id);
        modelBuilder.Entity<User>().HasKey(u => u.Id);

        // if (_isTestEnvironment)
        // {
        SeedData(modelBuilder);
        // }
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries().Where(e => e.Entity is Transaction || e.Entity is User || e.Entity is Category || e.Entity is TransactionType);
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                ((dynamic)entry.Entity).CreatedAt = DateTime.UtcNow;
                ((dynamic)entry.Entity).UpdatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                ((dynamic)entry.Entity).UpdatedAt = DateTime.UtcNow;
            }
        }
    }

    public async Task EnsureDatabaseCreatedAndMigratedAsync()
    {
        if (_isTestEnvironment)
        {
            await Database.EnsureDeletedAsync();
            await Database.EnsureCreatedAsync();
            await Database.MigrateAsync();
        }
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        var transactionTypes = LoadTransactionTypesFromCsv(Path.Combine(_dataFilePath, "SeedData", "TransactionTypes.csv"));
        modelBuilder.Entity<TransactionType>().HasData(transactionTypes);

        var categories = LoadCategoriesFromCsv(Path.Combine(_dataFilePath, "SeedData", "Categories.csv"), transactionTypes);
        modelBuilder.Entity<Category>().HasData(categories);

        var users = LoadUsersFromCsv(Path.Combine(_dataFilePath, "SeedData", "Users.csv"));
        modelBuilder.Entity<User>().HasData(users);

        var transactions = LoadTransactionsFromCsv(Path.Combine(_dataFilePath, "SeedData", "Transactions.csv"), users, categories);
        modelBuilder.Entity<Transaction>().HasData(transactions);

        var budgetLimits = LoadBudgetLimitsFromCsv(Path.Combine(_dataFilePath, "SeedData", "BudgetLimits.csv"), users, categories);
        modelBuilder.Entity<BudgetLimit>().HasData(budgetLimits);
    }

    private List<TransactionType> LoadTransactionTypesFromCsv(string filePath)
    {
        var transactionTypes = new List<TransactionType>();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines.Skip(1))
        {
            var values = ParseCsvLine(line);
            if (values.Length < 2)
            {
                Console.WriteLine("Not enough elements in TransactionTypes");
                continue;
            }

            var transactionType = new TransactionType
            {
                Id = values[0],
                Name = values[1],
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            transactionTypes.Add(transactionType);
        }

        return transactionTypes;
    }

    private List<Category> LoadCategoriesFromCsv(string filePath, List<TransactionType> transactionTypes)
    {
        var categories = new List<Category>();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines.Skip(1))
        {
            var values = ParseCsvLine(line);
            if (values.Length < 4)
            {
                throw new Exception($"Not enough elements in Categories CSV line: {line}");
            }

            var transactionType = transactionTypes.FirstOrDefault(tt => tt.Id == values[3]);
            if (transactionType == null)
            {
                throw new Exception($"No matching transaction type found for TransactionTypeId: {values[3]} in line: {line}");
            }

            var category = new Category
            {
                Id = values[0],
                Name = values[1],
                ParentId = string.IsNullOrEmpty(values[2]) ? null : values[2],
                TransactionTypeId = transactionType.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            categories.Add(category);
        }

        return categories;
    }

    private List<User> LoadUsersFromCsv(string filePath)
    {
        var users = new List<User>();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines.Skip(1))
        {
            var values = ParseCsvLine(line);
            if (values.Length < 5)
            {
                throw new Exception($"Not enough elements in Users CSV line: {line}");
            }

            var user = new User
            {
                Id = long.Parse(values[0]),
                Email = values[1],
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(values[2]),
                FirstName = values[3],
                LastName = string.IsNullOrEmpty(values[4]) ? null : values[4],
            };

            users.Add(user);
        }

        return users;
    }

    private List<Transaction> LoadTransactionsFromCsv(string filePath, List<User> users, List<Category> categories)
    {
        var transactions = new List<Transaction>();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines.Skip(1))
        {
            var values = ParseCsvLine(line);
            if (values.Length < 8)
            {
                throw new Exception($"Not enough elements in Transactions CSV line: {line}");
            }

            var user = users.FirstOrDefault(u => u.Id == long.Parse(values[1]));
            if (user == null)
            {
                throw new Exception($"No matching user found for UserId: {values[1]} in line: {line}");
            }

            var category = categories.FirstOrDefault(c => c.Id == values[7]);
            if (category == null)
            {
                throw new Exception($"No matching category found for CategoryId: {values[7]} in line: {line}");
            }

            var transaction = new Transaction
            {
                Id = long.Parse(values[0]),
                UserId = user.Id,
                Date = DateOnly.Parse(values[2]),
                OriginalDescription = values[3],
                ModifiedDescription = string.IsNullOrEmpty(values[4]) ? null : values[4],
                Amount = decimal.Parse(values[5]),
                Currency = values[6],
                CategoryId = category.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            transactions.Add(transaction);
        }

        return transactions;
    }

    private List<BudgetLimit> LoadBudgetLimitsFromCsv(string filePath, List<User> users, List<Category> categories)
    {
        var budgetLimits = new List<BudgetLimit>();
        var lines = File.ReadAllLines(filePath);

        var idx = 1;

        foreach (var line in lines.Skip(1))
        {
            var values = ParseCsvLine(line);
            if (values.Length < 4)
            {
                throw new Exception($"Not enough elements in BudgetLimits CSV line: {line}");
            }

            var category = categories.FirstOrDefault(c => c.Id == values[0]);
            if (category == null)
            {
                throw new Exception($"No matching category found for CategoryId: {values[0]} in line: {line}");
            }

            var user = users.FirstOrDefault(u => u.Id == long.Parse(values[1]));
            if (user == null)
            {
                throw new Exception($"No matching user found for UserId: {values[1]} in line: {line}");
            }

            var budgetLimit = new BudgetLimit
            {
                Id = idx++,
                CategoryId = category.Id,
                UserId = user.Id,
                PeriodType = values[2],
                Amount = decimal.Parse(values[3]),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            budgetLimits.Add(budgetLimit);
        }

        return budgetLimits;
    }

    private string[] ParseCsvLine(string line)
    {
        using (var parser = new TextFieldParser(new StringReader(line)))
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            parser.HasFieldsEnclosedInQuotes = true;

            var fields = parser.ReadFields();
            return fields ?? Array.Empty<string>();
        }
    }
}