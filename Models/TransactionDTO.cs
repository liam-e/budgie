namespace BudgetApi.Models;

public class TransactionDTO
{
    public required DateOnly Date { get; set; }
    public required string Description { get; set; }
    public required float Amount { get; set; }
    public string? CategoryName { get; set; }

    public static TransactionDTO MapFromTransaction(Transaction transaction)
    {
        return new TransactionDTO
        {
            Date = transaction.Date,
            Description = transaction.Description,
            Amount = transaction.Amount,
            CategoryName = transaction.Category?.Name
        };
    }
}