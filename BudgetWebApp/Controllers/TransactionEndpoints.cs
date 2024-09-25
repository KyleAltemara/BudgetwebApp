using Microsoft.EntityFrameworkCore;
using BudgetWebApp.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using BudgetWebApp.Models;

namespace BudgetWebApp.Controllers;

public static class TransactionEndpoints
{
    public static void MapTransactionEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Transaction").WithTags(nameof(Transaction));

        group.MapGet("/", async (BudgetWebAppContext db) =>
        {
            return await db.Transactions.ToListAsync();
        })
        .WithName("GetAllTransactions")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Transaction>, NotFound>> (int id, BudgetWebAppContext db) =>
        {
            var transaction = await db.Transactions.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id);
            if (transaction == null)
            {
                return TypedResults.NotFound();
            }

            transaction.Category = await db.Categories.FindAsync(transaction.CategoryId);
            return TypedResults.Ok(transaction);
        })
        .WithName("GetTransactionById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Transaction transaction, BudgetWebAppContext db) =>
        {
            var affected = await db.Transactions
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, transaction.Id)
                    .SetProperty(m => m.Amount, transaction.Amount)
                    .SetProperty(m => m.Date, transaction.Date)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateTransaction")
        .WithOpenApi();

        group.MapPost("/", async (Transaction transaction, BudgetWebAppContext db) =>
        {
            db.Transactions.Add(transaction);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Transaction/{transaction.Id}",transaction);
        })
        .WithName("CreateTransaction")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, BudgetWebAppContext db) =>
        {
            var affected = await db.Transactions
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteTransaction")
        .WithOpenApi();
    }
}
