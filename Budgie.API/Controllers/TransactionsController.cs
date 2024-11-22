using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Budgie.API.Models;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;

namespace Budgie.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly BudgetContext _context;

    public TransactionsController(BudgetContext context)
    {
        _context = context;
    }

    // GET: api/Transactions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetTransactions()
    {
        if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
        {
            return BadRequest(new { error = "Invalid user ID format" });
        }

        var transactions = await _context.Transactions
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Date)
            .Select(t => new TransactionDTO
            {
                Date = t.Date,
                OriginalDescription = t.OriginalDescription,
                ModifiedDescription = t.ModifiedDescription,
                Amount = t.Amount,
                Currency = t.Currency,
                CategoryId = t.CategoryId,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToListAsync();

        return Ok(new { transactions });
    }

    // PUT: api/Transactions/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTransaction(long id, Transaction transaction)
    {
        if (id != transaction.Id)
        {
            return BadRequest(new { error = "ID does not match transcation ID." });
        }
        if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
        {
            return BadRequest(new { error = "Invalid user ID format." });
        }
        if (transaction.UserId != userId)
        {
            return Forbid();
        }

        var existingTransaction = await _context.Transactions
            .Include(t => t.Category)
            .ThenInclude(c => c!.TransactionType)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (existingTransaction == null)
        {
            return NotFound();
        }

        // Check if the category change is allowed for transfer transactions
        if (existingTransaction.Category?.TransactionType?.Id == "transfer" && existingTransaction.CategoryId != transaction.CategoryId)
        {
            return BadRequest("Transfer transactions cannot change category.");
        }

        // Validate amount consistency with transaction type
        var newCategory = await _context.Categories.Include(c => c.TransactionType).FirstOrDefaultAsync(c => c.Id == transaction.CategoryId);
        if (newCategory == null)
        {
            return BadRequest("Invalid category.");
        }

        var newTransactionType = newCategory.TransactionType;

        if (newTransactionType == null)  // Added null check
        {
            return BadRequest("Invalid transaction type.");
        }

        if (transaction.Amount < 0 && (newTransactionType.Id == "direct-credit" || newTransactionType.Id == "refund"))
        {
            return BadRequest("Negative amounts cannot be direct credits or refunds.");
        }

        if (transaction.Amount > 0 && (newTransactionType.Id == "purchase" || newTransactionType.Id == "international-purchase"))
        {
            return BadRequest("Positive amounts cannot be purchases or international purchases.");
        }

        // Update transaction
        _context.Entry(existingTransaction).CurrentValues.SetValues(transaction);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TransactionExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Transactions
    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactionCreateDTO transactionDTO)
    {
        if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
        {
            return BadRequest(new { error = "Invalid user ID format" });
        }

        // Map the DTO to the Transaction model
        var transaction = new Transaction
        {
            UserId = userId, // Automatically set from the authenticated user
            Date = transactionDTO.Date,
            OriginalDescription = transactionDTO.Description,
            ModifiedDescription = "",
            Amount = transactionDTO.Amount,
            Currency = transactionDTO.Currency,
            CategoryId = transactionDTO.CategoryId!,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Add the new transaction to the database
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();


        return Ok(new { transaction });
    }

    [HttpPost("UploadCsv")]
    public async Task<ActionResult> UploadCsv(IFormFile file)
    {
        if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
        {
            return BadRequest(new { error = "Invalid user ID format" });
        }

        if (file.Length == 0)
        {
            return BadRequest("The CSV file is empty.");
        }

        try
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var textFieldParser = new TextFieldParser(reader))
            {
                textFieldParser.TextFieldType = FieldType.Delimited;
                textFieldParser.SetDelimiters(",");

                bool isHeader = true;

                while (!textFieldParser.EndOfData)
                {
                    if (isHeader)
                    {
                        textFieldParser.ReadFields();
                        isHeader = false;
                        continue;
                    }

                    string[] fields = textFieldParser.ReadFields()!;

                    if (fields.Length < 5)
                    {
                        return BadRequest("The CSV file is formatted incorrectly.");
                    }

                    if (!DateOnly.TryParseExact(fields[0], "dd MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly date))
                    {
                        return BadRequest($"Failed to parse date: {fields[0]}");
                    }

                    string originalDescription = fields[1];
                    string amountString = fields[3];
                    string balance = fields[4];

                    if (!decimal.TryParse(amountString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount))
                    {
                        return BadRequest($"Failed to parse amount: {amountString}");
                    }

                    var transaction = new Transaction
                    {
                        UserId = userId,
                        Date = date,
                        OriginalDescription = originalDescription,
                        Amount = amount,
                        Currency = "AUD",
                        CategoryId = "none",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    transaction.ModifiedDescription = ModifyDescription(transaction.OriginalDescription);

                    _context.Transactions.Add(transaction);

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to add transaction {transaction.Date}, {transaction.OriginalDescription}, {transaction.Amount}: {ex.Message}");
                    }
                }
            }

            return Ok("File uploaded and transactions added successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while processing the CSV file: {ex.Message}");
        }
    }

    // GET: api/Transactions/Categories
    [HttpGet("Categories")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
    {
        var categories = await _context.Categories
            .Include(c => c.TransactionType)
            .Select(c => new CategoryDTO
            {
                Id = c.Id,
                ParentId = c.ParentId,
                Name = c.Name,
                TransactionTypeName = c.TransactionType!.Name,
                TransactionTypeId = c.TransactionType!.Id
            })
            .ToListAsync();

        return Ok(categories);
    }


    private string? ModifyDescription(string originalDescription)
    {
        return originalDescription;
    }


    private bool TransactionExists(long id)
    {
        return _context.Transactions.Any(e => e.Id == id);
    }
}