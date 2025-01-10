using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budgie.API.Models;

using System.ComponentModel.DataAnnotations;

public class ValidSourceAttribute : ValidationAttribute
{
    private readonly string[] _allowedSources = [
        "Up"
    ];

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string source && _allowedSources.Contains(source))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult($"The Source must be one of the following: {string.Join(", ", _allowedSources)}.");
    }
}

public class IntegrationKey
{
    [Key]
    public long Id { get; set; }

    [ForeignKey("UserId")]
    public required long UserId { get; set; }

    public required User User { get; set; }

    public required string Source { get; set; }

    public required string EncryptedKey { get; set; }

    public required string AccountId { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt { get; set; }
}

public class IntegrationKeyDTO
{
    [ValidSource]
    public required string Source { get; set; }

    public required string Key { get; set; }

    public required string AccountId { get; set; }
}
