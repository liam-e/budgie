using System.Security.Claims;
using System.Text;
using BudgetApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace BudgetApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    public static User user = new User();
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("register")]
    public ActionResult<User> Register(UserDto request)
    {
        user.Username = request.Username;
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        return Ok(user);
    }

    [HttpPost("login")]
    public ActionResult<User> Login(UserDto request)
    {
        if (user.Username != request.Username)
        {
            return BadRequest("User not found");
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
        List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.Username)
        };

        Console.WriteLine(_configuration.GetSection("AppSettings:Token").Value);

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!
            )
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}