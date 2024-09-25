namespace BudgetWebApp.Models;

public class Transaction
{
    public int Id { get; set; }
    public Category? Category { get; set; }
    public double Amount { get; set; }
    public DateOnly Date { get; set; }
}
