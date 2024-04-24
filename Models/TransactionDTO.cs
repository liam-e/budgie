namespace BudgetApi.Models;

public class TransactionDTO
{
    public long Id { get; set; }
    public required DateOnly Date { get; set; }
    public required string Description { get; set; }
    public required float Amount { get; set; }
    public required float RunningTotal { get; set; }

    public static TransactionDTO MapFromTransaction(Transaction transaction)
    {
        return new TransactionDTO
        {
            Id = transaction.Id,
            Date = transaction.Date,
            Description = transaction.Description,
            Amount = transaction.Amount,
            RunningTotal = transaction.RunningTotal
        };
    }
}