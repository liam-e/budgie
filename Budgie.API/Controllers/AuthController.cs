using System.Security.Claims;
using System.Text;
using Budgie.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;

namespace Budgie.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly BudgetContext _context;
    private readonly IConfiguration _configuration;
    private readonly CookieOptions cookieOptions;

    private readonly TimeSpan accessTokenTimeSpan = TimeSpan.FromMinutes(5);
    private readonly TimeSpan refreshTokenTimeSpan = TimeSpan.FromDays(28);
    private readonly TimeSpan tokenExpiryTimeSpan = TimeSpan.FromDays(-999);

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

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Register(UserRegisterDTO request)
    {
        if (!IsValidEmail(request.Email))
        {
            return BadRequest(new { error = "Invalid email format" });
        }

        if (await UserExists(request.Email))
        {
            return Conflict(new { error = "The email already exists." });
        }

        if (string.IsNullOrWhiteSpace(request.FirstName))
        {
            return BadRequest(new { error = "First name is required." });
        }

        User user = new()
        {
            // Id = Guid.NewGuid().ToString(),
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        GenerateTokens(user, HttpContext);

        return Ok(new { message = "Registration successful and user is now logged in" });
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Login(UserLoginDTO request)
    {
        if (!IsValidEmail(request.Email))
        {
            return BadRequest(new { error = "Invalid email format" });
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { error = "Password is required." });
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return BadRequest(new { error = "Login unsuccessful" });
        }

        GenerateTokens(user, HttpContext);

        return Ok(new { message = "Login successful" });
    }

    [HttpPost("refresh")]
    public IActionResult RefreshToken()
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        {
            return Unauthorized(new { error = "Invalid refresh token" });
        }

        var principal = GetPrincipalFromExpiredToken(refreshToken);
        if (principal == null)
        {
            return Unauthorized(new { error = "Invalid refresh token" });
        }

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { error = "Invalid user ID format" });
        }

        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return Unauthorized(new { error = "Invalid user" });
        }


        GenerateTokens(user, HttpContext);

        return Ok(new { message = "Token has been refreshed" });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        ExpireToken(HttpContext);
        ExpireToken(HttpContext, isRefreshToken: true);

        return Ok(new { message = "Logged out successfully" });
    }

    [HttpGet("status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Status()
    {
        if (HttpContext.User.Identity?.IsAuthenticated == true)
        {
            return Ok(new { message = "User is logged in" });
        }

        return Unauthorized(new { error = "User is not logged in" });
    }

    private void GenerateTokens(User user, HttpContext context)
    {
        GenerateToken(user, context); // accessToken
        GenerateToken(user, context, isRefreshToken: true); // refreshToken
    }

    private void GenerateToken(User user, HttpContext context, bool isRefreshToken = false)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("FirstName", user.FirstName),
        };

        if (!isRefreshToken) claims.Add(new Claim(ClaimTypes.Role, "User"));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value!));

        string token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
            claims: claims,
            expires: isRefreshToken ? DateTime.UtcNow.Add(refreshTokenTimeSpan) : DateTime.UtcNow.Add(accessTokenTimeSpan),
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
            Path = isRefreshToken ? "/api/auth/refresh" : "/"
        });
    }

    private void ExpireToken(HttpContext context, bool isRefreshToken = false)
    {
        // Create a cookie options with past expiration
        var expiredCookieOptions = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddDays(-1),  // Expire immediately
            HttpOnly = cookieOptions.HttpOnly,
            IsEssential = cookieOptions.IsEssential,
            Secure = cookieOptions.Secure,
            SameSite = cookieOptions.SameSite,
            Path = isRefreshToken ? "/api/auth/refresh" : "/"
        };

        // Append expired cookies to clear them
        context.Response.Cookies.Append(isRefreshToken ? "refreshToken" : "accessToken", "", expiredCookieOptions);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
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

            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature, StringComparison.InvariantCultureIgnoreCase))
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
