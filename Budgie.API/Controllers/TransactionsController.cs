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
                Id = t.Id,
                UserId = userId,
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
    public async Task<IActionResult> PutTransaction(long id, TransactionDTO transactionDTO)
    {
        if (id != transactionDTO.Id)
        {
            return BadRequest(new { error = "ID does not match transcation ID." });
        }
        if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
        {
            return BadRequest(new { error = "Invalid user ID format." });
        }
        if (transactionDTO.UserId != userId)
        {
            return Forbid();
        }

        var transaction = await _context.Transactions
            .Include(t => t.Category)
            .ThenInclude(c => c!.TransactionType)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (transaction == null)
        {
            return NotFound();
        }

        if (transaction.UserId != userId)
        {
            return Forbid();
        }

        var category = await _context.Categories
            .Include(c => c.TransactionType)
            .FirstOrDefaultAsync(c => c.Id == transactionDTO.CategoryId);

        var validationError = ValidateTransaction(transactionDTO, category);
        if (validationError != null)
        {
            return BadRequest(new { error = validationError });
        }

        transaction.Date = transactionDTO.Date;
        transaction.OriginalDescription = transactionDTO.OriginalDescription;
        transaction.ModifiedDescription = transactionDTO.ModifiedDescription;
        transaction.Amount = transactionDTO.Amount;
        transaction.Currency = transactionDTO.Currency;
        transaction.CategoryId = transactionDTO.CategoryId;
        transaction.UpdatedAt = DateTime.UtcNow;

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
            throw;
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

        var category = await _context.Categories
            .Include(c => c.TransactionType)
            .FirstOrDefaultAsync(c => c.Id == transactionDTO.CategoryId);

        var validationError = ValidateTransaction(transactionDTO, category);
        if (validationError != null)
        {
            return BadRequest(new { error = validationError });
        }

        var transaction = new Transaction
        {
            UserId = userId,
            Date = transactionDTO.Date,
            OriginalDescription = transactionDTO.Description,
            ModifiedDescription = "",
            Amount = transactionDTO.Amount,
            Currency = transactionDTO.Currency,
            CategoryId = transactionDTO.CategoryId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.Transactions.AddAsync(transaction);
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

        var numTransactionsAdded = 0;

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
                        numTransactionsAdded++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to add transaction {transaction.Date}, {transaction.OriginalDescription}, {transaction.Amount}: {ex.Message}");
                    }
                }
            }

            return Ok($"Uploaded successfully, {numTransactionsAdded} transactions added.");
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

    private string? ValidateTransaction(object transactionDTO, Category? category)
    {
        if (category == null)
        {
            return "Invalid category.";
        }

        if (category.TransactionType == null)
        {
            return "Invalid transaction type.";
        }

        decimal amount = transactionDTO is TransactionCreateDTO createDto ? createDto.Amount : ((TransactionDTO)transactionDTO).Amount;

        if (amount == 0)
        {
            return "Transaction amount cannot be zero.";
        }

        if (category.TransactionType.Name.Equals("expense", StringComparison.OrdinalIgnoreCase) && amount > 0)
        {
            return "Expenses cannot have a positive amount.";
        }

        if (category.TransactionType.Name.Equals("credit", StringComparison.OrdinalIgnoreCase) && amount < 0)
        {
            return "Credits cannot have a negative amount.";
        }

        return null;
    }

    private string ModifyDescription(string originalDescription)
    {
        // TODO: Implement method
        return originalDescription;
    }

    private bool TransactionExists(long id)
    {
        return _context.Transactions.Any(e => e.Id == id);
    }
}
