using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetApi.Models;
using System.Security.Claims;
using System.Globalization;
using Microsoft.VisualBasic.FileIO;
using Microsoft.AspNetCore.Authorization;

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
    public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetTransactions()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var transactions = await _context.Transactions
            .Where(t => t.UserId == userId)
            .ToListAsync();

        return transactions.Select(TransactionDTO.MapFromTransaction).ToList();
    }
    // GET: api/Transactions/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionDTO>> GetTransaction(long id)
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
        return TransactionDTO.MapFromTransaction(transaction);
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

    [HttpPost("UploadCsv")]
    public async Task<ActionResult> UploadCsv(IFormFile file)
    {

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return BadRequest("user id is null.");
        }

        if (file.Length == 0)
        {
            return BadRequest("The CSV file is empty.");
        }

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

                if (fields.Length != 5)
                {
                    return BadRequest("The CSV file is formatted incorrectly.");
                }

                if (!DateOnly.TryParseExact(fields[0], "dd MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly date))
                {
                    return BadRequest($"Failed to parse date: {fields[0]}");
                }

                var transaction = new Transaction
                {
                    UserId = userId,
                    Date = date,
                    Description = fields[1],
                    Amount = float.Parse(fields[3]),
                    RunningTotal = float.Parse(fields[4])
                };

                _context.Transactions.Add(transaction);
            }
        }
        await _context.SaveChangesAsync();

        Console.WriteLine("File uploaded successfully!");

        return Ok();
    }

    [HttpGet("testisauthorized")]
    public ActionResult TestIsAuthorized()
    {
        return Ok("The user is authorized.");
    }
}
