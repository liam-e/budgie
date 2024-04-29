using Microsoft.EntityFrameworkCore;

namespace BudgetApi.Models;

public class BudgetContext : DbContext
{
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=127.0.0.1;Username=postgres;Password=insecure_password;Database=budget");
}