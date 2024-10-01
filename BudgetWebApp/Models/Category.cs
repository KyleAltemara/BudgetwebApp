using System.ComponentModel.DataAnnotations;

namespace BudgetWebApp.Models;

public class Category
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    public ICollection<Transaction>? Transactions { get; set; }
}
