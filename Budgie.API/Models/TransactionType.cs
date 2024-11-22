using System.ComponentModel.DataAnnotations;

namespace Budgie.API.Models;

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