using Microsoft.EntityFrameworkCore;

namespace BudgetApi.Models;

[Index(nameof(Name), IsUnique = true)]
public class Category
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public ICollection<Transaction>? Transactions { get; set; } // Navigation property
}