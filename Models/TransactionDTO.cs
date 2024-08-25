using BudgetApi.Enums;

namespace BudgetApi.Models;

public class TransactionDTO
{
    public required DateOnly Date { get; set; }
    public required string Description { get; set; }
    public required decimal Amount { get; set; }
    public string? CategoryName { get; set; }
    public required string Type { get; set; }

    public static TransactionDTO MapFromTransaction(Transaction t)
    {
        return new TransactionDTO
        {
            Date = t.Date,
            Description = t.Description,
            Amount = t.Amount,
            CategoryName = t.Category?.Name,
            Type = t.Type.ToString()
        };
    }
}