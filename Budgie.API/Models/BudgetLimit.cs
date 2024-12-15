using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Budgie.API.Models;

public class BudgetLimit
{
    [Key]
    public long Id { get; set; } // Primary key

    [Required]
    public required string CategoryId { get; set; } // Foreign key

    [JsonIgnore]
    public Category? Category { get; set; } // Navigation property

    [Required]
    public required long UserId { get; set; } // Foreign key

    [JsonIgnore]
    public User? User { get; set; } // Navigation property

    [Required]
    public required string PeriodType { get; set; } // weekly, monthly, quarterly, annual

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public required decimal Amount { get; set; } // Limit amount

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt { get; set; }
}

public class BudgetLimitDTO
{
    public required string CategoryId { get; set; }
    public required long UserId { get; set; }
    public required string PeriodType { get; set; } // weekly, monthly, quarterly, annual
    public required decimal Amount { get; set; } // Limit amount
}

public class BudgetLimitCreateDTO
{
    public required string CategoryId { get; set; }
    public required string PeriodType { get; set; } // weekly, monthly, quarterly, annual
    public required decimal Amount { get; set; } // Limit amount
}