using System.Security.Claims;
using System.Text;
using BudgetApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

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
    public async Task<User> Register(UserDTO request)
    {
        User user = new User {
            Id = Guid.NewGuid().ToString(),
            Email = request.Email,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    [HttpPost("login")]
    public ActionResult<User> Login(UserDTO request)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

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
            new Claim(ClaimTypes.Email, user.Email),
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
}
