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
    public class BudgetLimitsController(BudgetContext context) : ControllerBase
    {
        private readonly BudgetContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BudgetLimit>>> GetBudgetLimits()
        {
            try
            {
                if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
                {
                    return Unauthorized(new { error = "User ID is invalid or missing." });
                }

                var budgetLimits = await _context.BudgetLimits
                    .Where(b => b.UserId == userId)
                    .Include(b => b.Category)
                    .ToListAsync();

                return Ok(budgetLimits);
            }
            catch (Exception)
            {
                // Log the exception
                return StatusCode(500, new { error = "An error occurred while fetching budget limits." });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BudgetLimitDTO>> CreateBudgetLimit([FromBody] BudgetLimitCreateDTO budgetLimitDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Invalid data format." });
            }

            try
            {
                if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
                {
                    return Unauthorized(new { error = "User ID is invalid or missing." });
                }

                var category = await _context.Categories.FindAsync(budgetLimitDto.CategoryId);
                if (category == null)
                {
                    return BadRequest(new { error = "Invalid category ID." });
                }

                if (!ValidateAmount(budgetLimitDto.Amount, category.TransactionTypeId))
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
            catch (Exception)
            {
                // Log the exception
                return StatusCode(500, new { error = "An error occurred while creating the budget limit." });
            }
        }

        private bool ValidateAmount(decimal amount, string transactionTypeId)
        {
            return transactionTypeId switch
            {
                "expense" => amount <= 0,
                "credit" => amount >= 0,
                _ => true // Allow other types for future extensibility
            };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBudgetLimit(long id, [FromBody] BudgetLimit budgetLimit)
        {
            if (!ModelState.IsValid || id != budgetLimit.Id)
            {
                return BadRequest(new { error = "Invalid data or ID mismatch." });
            }

            try
            {
                if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
                {
                    return Unauthorized(new { error = "User ID is invalid or missing." });
                }

                var existingBudgetLimit = await _context.BudgetLimits.FindAsync(id);
                if (existingBudgetLimit == null || existingBudgetLimit.UserId != userId)
                {
                    return NotFound(new { error = "Budget limit not found or unauthorized." });
                }

                var category = await _context.Categories.FindAsync(budgetLimit.CategoryId);
                if (category == null || !ValidateAmount(budgetLimit.Amount, category.TransactionTypeId))
                {
                    return BadRequest(new { error = "Invalid category or amount." });
                }

                existingBudgetLimit.CategoryId = budgetLimit.CategoryId;
                existingBudgetLimit.PeriodType = budgetLimit.PeriodType;
                existingBudgetLimit.Amount = budgetLimit.Amount;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { error = "The budget limit was updated by another user. Please refresh and try again." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while updating the budget limit." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudgetLimit(long id)
        {
            try
            {
                if (!long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
                {
                    return Unauthorized(new { error = "User ID is invalid or missing." });
                }

                var budgetLimit = await _context.BudgetLimits.FirstOrDefaultAsync(bl => bl.Id == id && bl.UserId == userId);
                if (budgetLimit == null)
                {
                    return NotFound(new { error = "Budget limit not found or unauthorized." });
                }

                _context.BudgetLimits.Remove(budgetLimit);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Budget limit deleted successfully." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while deleting the budget limit." });
            }
        }

        private bool BudgetLimitExists(long id)
        {
            return _context.BudgetLimits.Any(b => b.Id == id);
        }
    }
}
