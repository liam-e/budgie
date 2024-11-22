using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Budgie.API.Models;

public class Category
{
    [Key]
    public required string Id { get; set; } // Primary key
    public required string Name { get; set; }
    public string? ParentId { get; set; } // Foreign key

    [JsonIgnore]
    public Category? Parent { get; set; } // Navigation property

    [JsonIgnore]
    public ICollection<Category> Children { get; set; } = new List<Category>(); // Navigation property

    public required string TransactionTypeId { get; set; } // Foreign key

    [JsonIgnore]
    public TransactionType? TransactionType { get; set; } // Navigation property

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class CategoryDTO
{
    public required string Id { get; set; }
    public string? ParentId { get; set; }
    public required string Name { get; set; }
    public required string TransactionTypeName { get; set; }
    public required string TransactionTypeId { get; set; }
}