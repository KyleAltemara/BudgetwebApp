﻿using BudgetWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetWebApp.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new BudgetWebAppContext(serviceProvider.GetRequiredService<DbContextOptions<BudgetWebAppContext>>()) ??
            throw new ArgumentNullException(nameof(serviceProvider), $"Null {typeof(BudgetWebAppContext)}");

        // Look for any Transactions.
        if (context.Categories?.Any() ?? throw new ArgumentNullException(nameof(serviceProvider), "Null Categories property"))
        {
            return;   // DB has been seeded
        }

        var categories = new Category[]
        {
            new() { Name = "Groceries" },
            new() { Name = "Entertainment" },
            new() { Name = "Rent" }
        };

        context.Categories?.AddRange(categories);
        context.Transactions?.AddRange([
            new Transaction
            {
                Name = "Smith's",
                Amount = 100.00,
                Date = DateOnly.Parse(DateTime.Today.AddDays(-2).ToLongDateString()),
                Category = categories[0]
            },
            new Transaction
            {
                Name = "Movie",
                Amount = 50.00,
                Date = DateOnly.Parse(DateTime.Today.AddDays(-1).ToLongDateString()),
                Category = categories[1]
            },
            new Transaction
            {
                Name = "Aparment",
                Amount = 75.00,
                Date = DateOnly.Parse(DateTime.Today.AddDays(0).ToLongDateString()),
                Category = categories[2]
            },
            ]);
        context.SaveChanges();
    }
}
