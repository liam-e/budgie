using System.Security.Claims;
using System.Text;
using BudgetApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace BudgetApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly BudgetContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(BudgetContext context, IConfiguration configuration)
    {
        _context = context;   
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<User> Register(UserDto request)
    {
        User newUser = new User {
            Id = Guid.NewGuid().ToString(),
            Username = request.Username,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return newUser;
    }

    [HttpPost("login")]
    public ActionResult<User> Login(UserDto request)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);

        if (user == null)
        {
            return BadRequest("User not found"); // TODO: Change these errors to be more vague for security
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return BadRequest("Wrong password");
        }

        string token = CreateToken(user);

        return Ok(token);
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = [
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "User")
        ];

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!
            )
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            issuer: _configuration.GetSection("AppSettings:Issuer").Value,
            audience: _configuration.GetSection("AppSettings:Audience").Value,
            signingCredentials: creds
        );

        var writtenToken = new JwtSecurityTokenHandler().WriteToken(token);

        return writtenToken;
    }

    [HttpGet("GetUsername"), Authorize]
    public ActionResult<string> GetUsername()
    {
        var userName = User?.Identity?.Name;
        return Ok(userName);
    }
}
