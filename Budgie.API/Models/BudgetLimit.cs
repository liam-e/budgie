using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Budgie.API.Models;

public class BudgetLimit
{
    [Key]
    public long Id { get; set; } // Primary key

    public required string CategoryId { get; set; } // Foreign key

    [JsonIgnore]
    public Category? Category { get; set; } // Navigation property

    public required long UserId { get; set; } // Foreign key

    [JsonIgnore]
    public User? User { get; set; } // Navigation property

    [RegularExpression("weekly|monthly|quarterly|annual", ErrorMessage = "Invalid period type.")]
    public required string PeriodType { get; set; } // weekly, monthly, quarterly, annual

    [Range(-999999.99, 999999.99)]
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

    [RegularExpression("weekly|monthly|quarterly|annual", ErrorMessage = "Invalid period type.")]
    public required string PeriodType { get; set; }

    [Range(-999999.99, 999999.99)]
    public required decimal Amount { get; set; }
}
