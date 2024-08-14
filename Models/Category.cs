using Microsoft.EntityFrameworkCore;

namespace BudgetApi.Models;

[Index(nameof(Name), IsUnique = true)]
public class Category
{
    public required long Id { get; set; }
    public required string Name { get; set; }
}