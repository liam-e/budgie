namespace BudgetApi.Models;

public class Transaction
{
    public required long Id { get; set; }
    public required string UserId { get; set; }
    public required string Description { get; set; }
    public required DateOnly Date { get; set; }
    public required float Amount { get; set; }
    public required float RunningTotal { get; set; }
}