using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Budgie.API.Models;

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
            .Include(t => t.Category)
            .ThenInclude(c => c!.TransactionType)
            .OrderByDescending(t => t.Date)
            .Select(t => new TransactionDTO
            {
                Date = t.Date,
                OriginalDescription = t.OriginalDescription,
                ModifiedDescription = t.ModifiedDescription,
                Amount = t.Amount,
                Currency = t.Currency,
                CategoryName = t.Category!.Name,
                TransactionTypeName = t.Category.TransactionType!.Name,
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
    public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
    {
        if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
        {
            return BadRequest(new { error = "Invalid user ID format" });
        }

        transaction.UserId = userId;

        var category = await _context.Categories.Include(c => c.TransactionType).FirstOrDefaultAsync(c => c.Id == transaction.CategoryId);
        if (category == null)
        {
            return BadRequest("Invalid category.");
        }

        var transactionType = category.TransactionType;

        if (transactionType == null)  // Added null check
        {
            return BadRequest("Invalid transaction type.");
        }

        if (transaction.Amount < 0 && (transactionType.Id == "direct-credit" || transactionType.Id == "refund"))
        {
            return BadRequest("Negative amounts cannot be direct credits or refunds.");
        }

        if (transaction.Amount > 0 && (transactionType.Id == "purchase" || transactionType.Id == "international-purchase"))
        {
            return BadRequest("Positive amounts cannot be purchases or international purchases.");
        }

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
    }

    private bool TransactionExists(long id)
    {
        return _context.Transactions.Any(e => e.Id == id);
    }
}