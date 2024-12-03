using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Budgie.API.Models;

namespace Budgie.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetLimitsController : ControllerBase
    {
        private readonly BudgetContext _context;

        public BudgetLimitsController(BudgetContext context)
        {
            _context = context;
        }

        // GET: api/BudgetLimits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BudgetLimit>>> GetBudgetLimits()
        {
            if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return BadRequest(new { error = "Invalid user ID format" });
            }

            var budgetLimits = await _context.BudgetLimits
                .Where(b => b.UserId == userId)
                .Include(b => b.Category)
                .ToListAsync();

            return Ok(budgetLimits);
        }

        [HttpPost]
        public async Task<ActionResult<BudgetLimitDTO>> CreateBudgetLimit([FromBody] BudgetLimitCreateDTO budgetLimitDto)
        {
            if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return BadRequest(new { error = "Invalid user ID format" });
            }

            var category = await _context.Categories.FindAsync(budgetLimitDto.CategoryId);
            if (category == null)
            {
                return BadRequest(new { error = "Invalid category ID" });
            }

            if ((category.TransactionTypeId == "expense" && budgetLimitDto.Amount > 0) ||
                (category.TransactionTypeId == "credit" && budgetLimitDto.Amount < 0))
            {
                return BadRequest(new { error = "Invalid budget limit amount for the selected category type." });
            }

            var budgetLimit = new BudgetLimit
            {
                UserId = userId,
                CategoryId = budgetLimitDto.CategoryId,
                PeriodType = budgetLimitDto.PeriodType,
                Amount = budgetLimitDto.Amount
            };

            _context.BudgetLimits.Add(budgetLimit);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBudgetLimits), new { id = budgetLimit.Id }, budgetLimit);
        }


        // PUT: api/BudgetLimits/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBudgetLimit(long id, [FromBody] BudgetLimit budgetLimit)
        {
            if (id != budgetLimit.Id)
            {
                return BadRequest(new { error = "ID does not match budget limit ID." });
            }

            if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return BadRequest(new { error = "Invalid user ID format." });
            }

            if (budgetLimit.UserId != userId)
            {
                return Forbid();
            }

            var category = await _context.Categories.FindAsync(budgetLimit.CategoryId);
            if (category == null)
            {
                return BadRequest(new { error = "Invalid category ID" });
            }

            if ((category.TransactionTypeId == "expense" && budgetLimit.Amount > 0) ||
                (category.TransactionTypeId == "credit" && budgetLimit.Amount < 0))
            {
                return BadRequest(new { error = "Invalid budget limit amount for the selected category type." });
            }

            _context.Entry(budgetLimit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetLimitExists(id))
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

        private bool BudgetLimitExists(long id)
        {
            return _context.BudgetLimits.Any(b => b.Id == id);
        }
    }
}
