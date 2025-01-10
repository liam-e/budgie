using System.Security.Claims;
using Budgie.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Budgie.API.Helpers;

namespace Budgie.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IntegrationKeysController : ControllerBase
{
    private readonly BudgetContext _context;
    private readonly IConfiguration _configuration;

    public IntegrationKeysController(BudgetContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> AddIntegrationKey([FromBody] IntegrationKeyDTO integrationKey)
    {
        if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
        {
            return BadRequest(new { error = "Invalid user ID format" });
        }

        var user = await _context.Users.Include(u => u.IntegrationKeys).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return NotFound(new { error = "User not found" });
        }

        string secretKey = _configuration.GetSection("AppSettings:SecretKey").Value!;

        var existingKey = user.IntegrationKeys.FirstOrDefault(t => t.Source == integrationKey.Source);
        if (existingKey != null)
        {
            return Conflict(new { error = $"An IntegrationKey for source '{integrationKey.Source}' already exists." });
        }

        var encryptedKey = EncryptionHelper.EncryptKey(integrationKey.Key, secretKey);

        user.IntegrationKeys.Add(new IntegrationKey
        {
            UserId = userId,
            User = user,
            Source = integrationKey.Source,
            EncryptedKey = encryptedKey,
            AccountId = integrationKey.AccountId
        });

        // Set the IntegrationKeySource to the source of the newly added key
        user.IntegrationKeySource = integrationKey.Source;

        await _context.SaveChangesAsync();

        return Ok(new { message = "IntegrationKey added successfully" });
    }

    [HttpGet]
    public async Task<IActionResult> GetIntegrationKey(string source)
    {
        if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
        {
            return BadRequest(new { error = "Invalid user ID format" });
        }

        var user = await _context.Users.Include(u => u.IntegrationKeys).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return NotFound(new { error = "User not found" });
        }

        var integrationKey = user.IntegrationKeys.FirstOrDefault(t => t.Source == source);
        if (integrationKey == null)
        {
            return NotFound(new { error = "IntegrationKey not found" });
        }

        string secretKey = _configuration.GetSection("AppSettings:SecretKey").Value!;

        var decryptedKey = EncryptionHelper.DecryptKey(integrationKey!.EncryptedKey, secretKey);

        return Ok(new { message = "IntegrationKey retrieved successfully", source, accountId = integrationKey.AccountId, key = decryptedKey });
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteIntegrationKey(string source)
    {
        if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
        {
            return BadRequest(new { error = "Invalid user ID format" });
        }

        var user = await _context.Users.Include(u => u.IntegrationKeys).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return NotFound(new { error = "User not found" });
        }

        var integrationKey = user.IntegrationKeys.FirstOrDefault(t => t.Source == source);
        if (integrationKey == null)
        {
            return NotFound(new { error = "IntegrationKey not found" });
        }

        // Delete the integration key
        user.IntegrationKeys.Remove(integrationKey);

        // Set the IntegrationKeySource to the next newest key if one exists
        var nextKey = user.IntegrationKeys.OrderByDescending(t => t.CreatedAt).FirstOrDefault();
        if (nextKey != null)
        {
            user.IntegrationKeySource = nextKey.Source;
        }
        else
        {
            // No other keys exist, reset the source
            user.IntegrationKeySource = null;
        }

        await _context.SaveChangesAsync();

        return Ok(new { message = "IntegrationKey deleted successfully" });
    }
}