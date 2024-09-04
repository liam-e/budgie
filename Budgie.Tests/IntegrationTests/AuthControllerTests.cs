namespace Budgie.Tests.IntegrationTests;

[Collection("Test collection")]
public class AuthControllerTests : IClassFixture<BudgieAPIFactory>
{
    private readonly HttpClient _client;
    private readonly BudgieAPIFactory _factory;

    public AuthControllerTests(BudgieAPIFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(); // Initialize HttpClient
    }

    [Fact]
    public async Task Test_Login_Valid()
    {
        Console.WriteLine("Started Test_Login_Valid");
        var requestContent = new StringContent(
            JsonSerializer.Serialize(new { Email = "user@example.com", Password = "password1" }),
            Encoding.UTF8, "application/json");

        Console.WriteLine("Sending login request");
        var response = await _client.PostAsync("/api/auth/login", requestContent);

        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Login successful", responseString);
    }

    [Fact]
    public async Task Test_Login_Invalid_Email()
    {
        var requestContent = new StringContent(
            JsonSerializer.Serialize(new { Email = "invalidemail", Password = "password1" }),
            Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/auth/login", requestContent);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("The Email field is not a valid e-mail address", responseString);
    }

    [Fact]
    public async Task Test_Register_Valid_Tokens()
    {
        var requestContent = new StringContent(
            JsonSerializer.Serialize(new { Email = "newuser9000@example.com", Password = "password123", ConfirmPassword = "password123", FirstName = "New", LastName = "User" }),
            Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/auth/register", requestContent);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        Assert.True(response.Headers.TryGetValues("Set-Cookie", out var cookies));

        var cookieHeaders = cookies.ToList();

        Assert.Contains(cookieHeaders, h => h.Contains("accessToken"));
        Assert.Contains(cookieHeaders, h => h.Contains("refreshToken"));
    }

    [Fact]
    public async Task Test_Register_Missing_FirstName()
    {
        var requestContent = new StringContent(
            JsonSerializer.Serialize(new { Email = "newuser2@example.com", Password = "password123", ConfirmPassword = "password123" }),
            Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/api/auth/register", requestContent);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("missing required properties, including the following: firstName", responseString);
    }

    [Fact]
    public async Task Test_RefreshToken_Valid()
    {
        var loginContent = new StringContent(
            JsonSerializer.Serialize(new { Email = "user@example.com", Password = "password1" }),
            Encoding.UTF8, "application/json");
        var loginResponse = await _client.PostAsync("/api/auth/login", loginContent);

        loginResponse.EnsureSuccessStatusCode();
        var cookies = loginResponse.Headers.GetValues("Set-Cookie");

        _client.DefaultRequestHeaders.Add("Cookie", string.Join("; ", cookies));

        var response = await _client.PostAsync("/api/auth/refresh", null);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        Assert.True(response.Headers.TryGetValues("Set-Cookie", out var newCookies));
        var newCookieHeaders = newCookies.ToList();

        Assert.Contains(newCookieHeaders, h => h.Contains("accessToken"));
        Assert.Contains(newCookieHeaders, h => h.Contains("refreshToken"));
    }

    [Fact]
    public async Task Test_Logout()
    {
        var loginContent = new StringContent(
            JsonSerializer.Serialize(new { Email = "user@example.com", Password = "password1" }),
            Encoding.UTF8, "application/json");
        var loginResponse = await _client.PostAsync("/api/auth/login", loginContent);

        loginResponse.EnsureSuccessStatusCode();
        var cookies = loginResponse.Headers.GetValues("Set-Cookie");

        _client.DefaultRequestHeaders.Add("Cookie", string.Join("; ", cookies));

        var response = await _client.PostAsync("/api/auth/logout", null);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Assert that cookies are cleared
        Assert.True(response.Headers.TryGetValues("Set-Cookie", out var expiredCookies));
        var expiredCookieHeaders = expiredCookies.ToList();

        Assert.All(expiredCookieHeaders, cookie =>
        {
            // Check that the cookies have the 'expires' attribute set in the past
            Assert.Contains("expires=", cookie.ToLower());  // Ensuring that the expiration is set
        });
    }
}
