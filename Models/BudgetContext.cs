using Microsoft.EntityFrameworkCore;

namespace BudgetApi.Models;

public class BudgetContext : DbContext
{
    public BudgetContext(DbContextOptions<BudgetContext> options) : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; } = null!;
}