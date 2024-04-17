using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BudgetApi.Controllers;
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
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        System.Diagnostics.Debug.WriteLine(userId);
        return await _context.Transactions
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }
    // GET: api/Transactions/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Transaction>> GetTransaction(long id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null)
        {
            return NotFound();
        }
        if (transaction.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier)!)
        {
            return Forbid();
        }
        return transaction;
    }
    // PUT: api/Transactions/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTransaction(long id, Transaction transaction)
    {
        if (id != transaction.Id)
        {
            return BadRequest();
        }
        if (transaction.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier)!)
        {
            return Forbid();
        }
        _context.Entry(transaction).State = EntityState.Modified;
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
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
    {
        transaction.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
    }
    // DELETE: api/Transactions/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(long id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null)
        {
            return NotFound();
        }
        if (transaction.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
        {
            return Forbid();
        }
        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    private bool TransactionExists(long id)
    {
        return _context.Transactions.Any(e => e.Id == id);
    }
}