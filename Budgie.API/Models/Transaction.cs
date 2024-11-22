using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budgie.API.Models;

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

    [RegularExpression("^[A-Z]{3}$", ErrorMessage = "Currency should be a valid 3-letter ISO code.")]
    public required string Currency { get; set; }

    public required string CategoryId { get; set; } // Foreign key
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

    [RegularExpression("^[A-Z]{3}$", ErrorMessage = "Currency should be a valid 3-letter ISO code.")]
    public required string Currency { get; set; }

    public required string CategoryId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class TransactionCreateDTO
{
    public required DateOnly Date { get; set; }
    public required string Description { get; set; }
    public required decimal Amount { get; set; }

    [RegularExpression("^[A-Z]{3}$", ErrorMessage = "Currency should be a valid 3-letter ISO code.")]
    public required string Currency { get; set; }

    public string? CategoryId { get; set; }
}