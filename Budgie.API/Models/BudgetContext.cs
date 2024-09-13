using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;

namespace Budgie.API.Models;

public class Category
{
    [Key]
    public required string Id { get; set; } // Primary key
    public required string Name { get; set; }
    public string? ParentId { get; set; } // Foreign key
    public Category? Parent { get; set; } // Navigation property
    public ICollection<Category> Children { get; set; } = new List<Category>(); // Navigation property
    public required string TransactionTypeId { get; set; } // Foreign key
    public TransactionType? TransactionType { get; set; } // Navigation property
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class TransactionType
{
    [Key]
    public required string Id { get; set; } // Primary key
    public required string Name { get; set; }
    public bool CanHaveCategory { get; set; }
    public ICollection<Category> Categories { get; set; } = new List<Category>(); // Navigation property
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class Transaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; } // Primary key

    public required long UserId { get; set; }
    public required DateOnly Date { get; set; }
    public required string OriginalDescription { get; set; }
    public string? ModifiedDescription { get; set; }
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }
    public required string CategoryId { get; set; } // Foreign key, required
    public Category? Category { get; set; } // Navigation property
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public TransactionType? TransactionType => Category?.TransactionType; // Navigation property from Category
}

public class TransactionDTO
{
    public required DateOnly Date { get; set; }
    public required string OriginalDescription { get; set; }
    public string? ModifiedDescription { get; set; }
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }
    public required string CategoryName { get; set; }
    public required string TransactionTypeName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static TransactionDTO MapFromTransaction(Transaction t)
    {
        return new TransactionDTO
        {
            Date = t.Date,
            OriginalDescription = t.OriginalDescription,
            ModifiedDescription = t.ModifiedDescription,
            Amount = t.Amount,
            Currency = t.Currency,
            CategoryName = t.Category!.Name,
            TransactionTypeName = t.TransactionType!.Name,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        };
    }
}

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; } // Primary key

    [EmailAddress]
    public required string Email { get; set; }

    public required string PasswordHash { get; set; }

    public required string FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class UserRegisterDTO
{
    [EmailAddress]
    [Required(ErrorMessage = "Email is required.")]
    public required string Email { get; set; }

    [MinLength(8)]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Password must have a minimum of 8 characters with at least one letter and one number.")]
    public required string Password { get; set; }

    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public required string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    public required string FirstName { get; set; } // Required first name

    public string? LastName { get; set; } // Optional last name
}

public class UserLoginDTO
{
    [EmailAddress]
    [Required(ErrorMessage = "Email is required.")]
    public required string Email { get; set; }

    [MinLength(8)]
    [Required(ErrorMessage = "Password is required.")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Password must have a minimum of 8 characters with at least one letter and one number.")]
    public required string Password { get; set; }
}

public class BudgetContext : DbContext
{
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<TransactionType> TransactionTypes { get; set; }

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
            .HasForeignKey(t => t.CategoryId)
            .IsRequired();

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
        var transactionTypes = LoadTransactionTypesFromCsv(Path.Combine(_dataFilePath, "SeedData", "transactiontypes.csv"));
        modelBuilder.Entity<TransactionType>().HasData(transactionTypes);

        var categories = LoadCategoriesFromCsv(Path.Combine(_dataFilePath, "SeedData", "categories.csv"), transactionTypes);
        modelBuilder.Entity<Category>().HasData(categories);

        var users = LoadUsersFromCsv(Path.Combine(_dataFilePath, "SeedData", "users.csv"));
        modelBuilder.Entity<User>().HasData(users);

        var transactions = LoadTransactionsFromCsv(Path.Combine(_dataFilePath, "SeedData", "transactions.csv"), users, categories);
        modelBuilder.Entity<Transaction>().HasData(transactions);
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