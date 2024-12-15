using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt { get; set; }
}

public class CategoryDTO
{
    public required string Id { get; set; }
    public string? ParentId { get; set; }
    public required string Name { get; set; }
    public required string TransactionTypeName { get; set; }
    public required string TransactionTypeId { get; set; }
}