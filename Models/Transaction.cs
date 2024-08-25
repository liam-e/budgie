using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BudgetApi.Enums;

namespace BudgetApi.Models;

public class Transaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public required string UserId { get; set; }
    public required DateOnly Date { get; set; }
    public required string Description { get; set; }
    public required decimal Amount { get; set; }
    public string? CategoryId { get; set; } // Foreign Key
    public Category? Category { get; set; } // Navigation property

    public required TransactionType Type { get; set; }
}