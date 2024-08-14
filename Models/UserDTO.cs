using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BudgetApi.Models;

[Index(nameof(Email), IsUnique = true)]
public class UserDTO
{
    [EmailAddress]
    public required string Email { get; set; }

    [MinLength(8)]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Password must have a minimum of 8 characters with at least one letter and one number.")]
    public required string Password { get; set; }

    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public required string ConfirmPassword { get; set; }
}