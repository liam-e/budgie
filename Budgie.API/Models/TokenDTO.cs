namespace BudgetApi.Models;

public class TokenDTO
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}