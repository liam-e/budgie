using System.Security.Claims;
using System.Text;
using BudgetApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;

namespace BudgetApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly BudgetContext _context;
    private readonly IConfiguration _configuration;
    private readonly CookieOptions cookieOptions;

    private readonly TimeSpan accessTokenTimeSpan = TimeSpan.FromDays(7);
    private readonly TimeSpan refreshTokenTimeSpan = TimeSpan.FromDays(28);
    private readonly TimeSpan tokenExpiryTimeSpan = TimeSpan.FromDays(-7);

    public AuthController(BudgetContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;

        cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/"
        };
    }

    // --   REGISTER   --
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Register(UserDTO request)
    {
        if (!IsValidEmail(request.Email))
        {
            Console.WriteLine("Error: invalid email format");
            return BadRequest(new { error = "Invalid email format" });
        }

        if (await UserExists(request.Email))
        {

            Console.WriteLine("Error: the email already exists");
            return Conflict(new { error = "The email already exists." });
        }

        User user = new()
        {
            Id = Guid.NewGuid().ToString(),
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        GenerateTokens(user, HttpContext);

        Console.WriteLine("Register successful and is logged in");
        return Ok(new { message = "Register successful and is now logged in" });
    }

    // --   LOGIN   --
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Login(UserDTO request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            Console.WriteLine("Login unsuccessful");
            return BadRequest(new { error = "Login unsuccessful" });
        }

        GenerateTokens(user, HttpContext);

        Console.WriteLine("Login successful");
        return Ok(new { message = "Login successful" });
    }

    // --   REFRESH   --
    [HttpPost("refresh")]
    public IActionResult RefreshToken()
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            Console.WriteLine("Error: Failed to retrieve refreshToken");
            return Unauthorized(new { error = "Invalid refresh token" });
        }

        var principal = GetPrincipalFromExpiredToken(refreshToken);
        if (principal == null)
        {
            Console.WriteLine("Error: failed to get principal from refreshToken");
            return Unauthorized(new { error = "Invalid refresh token" });
        }

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            Console.WriteLine("Error: userid in principal does not exist in database");
            return Unauthorized(new { error = "Invalid user" });
        }

        GenerateTokens(user, HttpContext);

        Console.WriteLine("Token has been refreshed");
        return Ok(new { message = "Token has been refreshed" });
    }

    // --   LOGOUT   --
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        ExpireToken(HttpContext);
        ExpireToken(HttpContext, isRefreshToken: true);

        Console.WriteLine("Logged out successfully");
        return Ok(new { message = "Logged out successfully" });
    }


    // --   STATUS   --
    [HttpGet("status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Status()
    {
        if (HttpContext.User.Identity?.IsAuthenticated == true)
        {

            Console.WriteLine("User is logged in");
            return Ok(new { message = "User is logged in" });
        }


        Console.WriteLine("Error: user is not logged in");
        return Unauthorized(new { error = "User is not logged in" });
    }

    private void GenerateTokens(User user, HttpContext context)
    {
        GenerateToken(user, context); // accessToken
        GenerateToken(user, context, isRefreshToken: true); // refreshToken
    }

    private void GenerateToken(User user, HttpContext context, bool isRefreshToken = false)
    {
        List<Claim> claims = [new Claim(ClaimTypes.NameIdentifier, user.Id), new Claim(ClaimTypes.Email, user.Email)];

        if (!isRefreshToken) claims.Add(new Claim(ClaimTypes.Role, "User"));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value!));

        string token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
            claims: claims,
            expires: isRefreshToken ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddMinutes(5),
            issuer: _configuration.GetSection("AppSettings:Issuer").Value,
            audience: _configuration.GetSection("AppSettings:Audience").Value,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
        ));

        context.Response.Cookies.Append(isRefreshToken ? "refreshToken" : "accessToken", token, new CookieOptions
        {
            MaxAge = isRefreshToken ? refreshTokenTimeSpan : accessTokenTimeSpan,
            HttpOnly = cookieOptions.HttpOnly,
            IsEssential = cookieOptions.IsEssential,
            Secure = cookieOptions.Secure,
            SameSite = cookieOptions.SameSite,
            Path = isRefreshToken ? "/api/Auth/refresh" : "/" // TODO: Limit refreshToken to /api/auth/refresh endpoint
        });

        Console.WriteLine(isRefreshToken ? "refreshToken generated" : "accessToken generated");
    }

    private void ExpireToken(HttpContext context, bool isRefreshToken = false)
    {
        context.Response.Cookies.Append(isRefreshToken ? "refreshToken" : "accessToken", "", new CookieOptions
        {
            MaxAge = tokenExpiryTimeSpan,
            HttpOnly = cookieOptions.HttpOnly,
            IsEssential = cookieOptions.IsEssential,
            Secure = cookieOptions.Secure,
            SameSite = cookieOptions.SameSite,
            Path = isRefreshToken ? "/api/Auth/refresh" : "/"
        });

        Console.WriteLine(isRefreshToken ? "refreshToken expired" : "accessToken expired");
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false, // For expired tokens
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration.GetSection("AppSettings:Issuer").Value,
                ValidAudience = _configuration.GetSection("AppSettings:Audience").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value!))
            };
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            var jwtToken = securityToken as JwtSecurityToken;

            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }
    private static bool IsValidEmail(string email)
    {
        try
        {
            var address = new System.Net.Mail.MailAddress(email);
            return address.Address == email;
        }
        catch
        {
            return false;
        }
    }
    private async Task<bool> UserExists(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
}
