using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetApi.Models;
using System.Security.Claims;
using System.Globalization;
using Microsoft.VisualBasic.FileIO;
using Microsoft.AspNetCore.Authorization;

namespace BudgetApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelloWorldController : ControllerBase
    {
        // GET: api/HelloWorld
        [HttpGet]
        public IActionResult GetHelloWorld()
        {
            var response = new { message = "Hello World" };
            return Ok(response);
        }
    }

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
                .Include(t => t.Category)
                .OrderByDescending(t => t.Date)
                // TODO: Use MapFromTransaction?
                .Select(t => new TransactionDTO
                {
                    Date = t.Date,
                    Description = t.Description,
                    Amount = t.Amount,
                    CategoryName = t.Category != null ? t.Category.Name : null,
                    Type = t.Type.ToString()
                })
                .ToListAsync();

            return Ok(new { transactions });
        }


        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDTO>> GetTransaction(long id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var transaction = await _context.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (transaction == null)
            {
                return NotFound();
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
        public async Task<ActionResult<IEnumerable<Transaction>>> PostTransaction(List<Transaction> transactions)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            foreach (Transaction transaction in transactions)
            {
                transaction.UserId = UserId;
                _context.Transactions.Add(transaction);
            }
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTransactions), transactions);
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
            // TODO: Put CSV logic into service to reuse to load mock data

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
                        Amount = decimal.Parse(fields[3]),
                        Type = Enums.TransactionType.Expense // TODO: Not all are expenses
                    };

                    _context.Transactions.Add(transaction);
                }
            }
            await _context.SaveChangesAsync();

            Console.WriteLine("File uploaded successfully!");

            return Ok();
        }
    }
}