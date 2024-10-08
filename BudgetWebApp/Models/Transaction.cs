﻿namespace BudgetWebApp.Models;

public class Transaction
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public double Amount { get; set; }
    public DateOnly Date { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
