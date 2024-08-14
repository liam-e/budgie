using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetApi.Models;

public class Transaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public required string UserId { get; set; }
    public required DateOnly Date { get; set; }
    public required string Description { get; set; }
    public required float Amount { get; set; }
    public required float RunningTotal { get; set; }
    public Category? Category { get; set; }
}