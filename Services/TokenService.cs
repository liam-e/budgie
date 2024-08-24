
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using BudgetApi.Models;

namespace BudgetApi.Services;

public class TokenService
{
    private readonly IConfiguration _configuration;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        _tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false, // For expired tokens
            ValidateIssuerSigningKey = true,
            ValidIssuer = _configuration.GetSection("AppSettings:Issuer").Value,
            ValidAudience = _configuration.GetSection("AppSettings:Audience").Value,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value!))
        };
    }

    public static void SetTokensInsideCookie(TokenDTO tokenDTO, HttpContext context)
    {
        context.Response.Cookies.Append("accessToken", tokenDTO.AccessToken,
        new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddMinutes(5),
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });

        context.Response.Cookies.Append("refreshToken", tokenDTO.RefreshToken,
        new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var securityToken);
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
}