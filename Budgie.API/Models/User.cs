using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budgie.API.Models;

public class User
{
    [Key]
    public long Id { get; set; } // Primary key

    [EmailAddress]
    public required string Email { get; set; }

    public required string PasswordHash { get; set; }

    public required string FirstName { get; set; }

    public string? LastName { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt { get; set; }

    public List<IntegrationKey> IntegrationKeys { get; set; } = [];


    [ValidSource]
    public string? IntegrationKeySource { get; set; }
}

public class UserRegisterDTO
{
    [EmailAddress]
    [Required(ErrorMessage = "Email is required.")]
    public required string Email { get; set; }

    [MinLength(8)]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Password must have a minimum of 8 characters with at least one letter and one number.")]
    public required string Password { get; set; }

    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public required string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    public required string FirstName { get; set; }

    public string? LastName { get; set; }
}

public class UserLoginDTO
{
    [EmailAddress]
    [Required(ErrorMessage = "Email is required.")]
    public required string Email { get; set; }

    [MinLength(8)]
    [Required(ErrorMessage = "Password is required.")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Password must have a minimum of 8 characters with at least one letter and one number.")]
    public required string Password { get; set; }
}

public class UserResponseDTO
{
    public long Id { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }

    [ValidSource]
    public string? IntegrationKeySource { get; set; }
}
