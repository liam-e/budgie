using System.Security.Claims;
using System.Text;
using BudgetApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using BudgetApi.Services;

namespace BudgetApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly BudgetContext _context;
    private readonly IConfiguration _configuration;
    private readonly TokenService _tokenService;

    public AuthController(BudgetContext context, IConfiguration configuration, TokenService tokenService)
    {
        _context = context;
        _configuration = configuration;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Register(UserDTO request)
    {
        if (!IsValidEmail(request.Email))
        {
            return BadRequest(new { error = "Invalid email format" });
        }

        if (_context.Users.Any(u => u.Email == request.Email))
        {
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

        // Short-lived token
        string accessToken = CreateToken(user, TimeSpan.FromMinutes(5));

        // Long-lived token
        string refreshToken = CreateToken(user, TimeSpan.FromDays(7), isRefreshToken: true);

        TokenService.SetTokensInsideCookie(new TokenDTO { AccessToken = accessToken, RefreshToken = refreshToken }, HttpContext);

        return Ok(new { accessToken });
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<User> Login(UserDTO request)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

        if (user == null)
        {
            return BadRequest(new { error = "Login unsuccessful" });
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return BadRequest(new { error = "Login unsuccessful" });
        }

        // Short-lived token
        string accessToken = CreateToken(user, TimeSpan.FromMinutes(5));

        // Long-lived token
        string refreshToken = CreateToken(user, TimeSpan.FromDays(7), isRefreshToken: true);

        TokenService.SetTokensInsideCookie(new TokenDTO { AccessToken = accessToken, RefreshToken = refreshToken }, HttpContext);

        return Ok(new { accessToken });
    }

    private string CreateToken(User user, TimeSpan expiry, bool isRefreshToken = false)
    {
        List<Claim> claims = [
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
        ];

        if (!isRefreshToken)
        {
            claims.Add(new Claim(ClaimTypes.Role, "User"));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:SecretKey").Value!
            )
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.Add(expiry),
            issuer: _configuration.GetSection("AppSettings:Issuer").Value,
            audience: _configuration.GetSection("AppSettings:Audience").Value,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpPost("refresh-token")]
    public ActionResult RefreshToken()
    {
        HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken);

        if (refreshToken == null)
        {
            return Unauthorized(new { error = "Invalid refresh token" });
        }

        var principal = _tokenService.GetPrincipalFromExpiredToken(refreshToken);
        if (principal == null)
        {
            return Unauthorized(new { error = "Invalid refresh token" });
        }

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return Unauthorized(new { error = "Invalid user" });
        }

        // Generate a new access token
        // TODO: refactor duplicate code
        string newAccessToken = CreateToken(user, TimeSpan.FromMinutes(5));
        string newRefreshToken = CreateToken(user, TimeSpan.FromDays(7), isRefreshToken: true);

        TokenService.SetTokensInsideCookie(new TokenDTO { AccessToken = newAccessToken, RefreshToken = refreshToken }, HttpContext);

        return Ok(new { accessToken = newAccessToken });
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
}
