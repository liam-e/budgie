using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budgie.API.Models;

public class TransactionType
{
    [Key]
    public required string Id { get; set; } // Primary key

    public required string Name { get; set; }
    public bool CanHaveCategory { get; set; }
    public ICollection<Category> Categories { get; set; } = new List<Category>(); // Navigation property

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt { get; set; }
}