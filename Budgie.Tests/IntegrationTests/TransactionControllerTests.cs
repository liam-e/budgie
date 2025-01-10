namespace Budgie.Tests.IntegrationTests;

[Collection("Test collection")]
public class TransactionsControllerTests : IClassFixture<BudgieAPIFactory>
{
    private readonly HttpClient _client;
    private readonly BudgieAPIFactory _factory;

    public TransactionsControllerTests(BudgieAPIFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();

        Authenticate().GetAwaiter().GetResult();
    }

    private async Task Authenticate()
    {
        var loginRequest = new
        {
            Email = "user@example.com",
            Password = "password1"
        };

        var requestContent = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/Auth/login", requestContent);
        response.EnsureSuccessStatusCode();

        // Extract cookies from the response and set them for future requests
        if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
        {
            _client.DefaultRequestHeaders.Add("Cookie", string.Join("; ", cookies));
        }
    }

    [Fact]
    public async Task Test_GetTransactions_Valid()
    {
        // Act - Get transactions request
        var response = await _client.GetAsync("/api/transactions");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseString);
        Assert.Contains("transactions", responseString);
    }

    [Fact]
    public async Task Test_PutTransaction_Valid()
    {
        // Arrange
        var transaction = new
        {
            Id = 1,
            UserId = 1,
            Date = "2023-08-01",
            Description = "Uber Eats Delivery",
            Amount = -45.60,
            Currency = "AUD",
            CategoryId = "restaurants-and-cafes"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(transaction), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/api/transactions/1", jsonContent);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Test_PutTransaction_Invalid_TransferCategoryChange()
    {
        // Arrange
        var transaction = new
        {
            Id = 49,
            UserId = 1,
            Date = "2023-08-17",
            Description = "TRANSFER TO FRIEND ;",
            Amount = 150.00,
            Currency = "AUD",
            CategoryId = "groceries" // Trying to change from transfer to groceries
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(transaction), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/api/transactions/49", jsonContent);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Transfer transactions cannot change category.", responseString);
    }

    [Fact]
    public async Task Test_PostTransaction_Invalid_NegativeAmountDirectCredit()
    {
        // Arrange
        var transaction = new
        {
            UserId = 1,
            Date = "2023-08-19",
            description = "Test DIRECT CREDIT NEGATIVE",
            Amount = -100.00,
            Currency = "AUD",
            CategoryId = "investments"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(transaction), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/transactions", jsonContent);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Negative amounts cannot be direct credits or refunds.", responseString);
    }

    [Fact]
    public async Task Test_PostTransaction_Invalid_PositiveAmountPurchase()
    {
        // Arrange
        var transaction = new
        {
            UserId = 1,
            Date = "2023-08-19",
            Description = "Test PURCHASE POSITIVE",
            Amount = 100.00,
            Currency = "AUD",
            CategoryId = "restaurants-and-cafes"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(transaction), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/transactions", jsonContent);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Positive amounts cannot be purchases or international purchases.", responseString);
    }
}
