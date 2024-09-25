using Microsoft.EntityFrameworkCore;
using BudgetWebApp.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using BudgetWebApp.Models;

namespace BudgetWebApp.Controllers;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Category").WithTags(nameof(Category));

        group.MapGet("/", async (BudgetWebAppContext db) =>
        {
            return await db.Categories.ToListAsync();
        })
        .WithName("GetAllCategories")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Category>, NotFound>> (int id, BudgetWebAppContext db) =>
        {
            var category = await db.Categories.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id);
            if (category == null)
            {
                return TypedResults.NotFound();
            }

            category.Transactions = await db.Transactions.Where(t => t.CategoryId == category.Id).ToListAsync();
            return TypedResults.Ok(category);
        })
        .WithName("GetCategoryById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Category category, BudgetWebAppContext db) =>
        {
            var affected = await db.Categories
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, category.Id)
                    .SetProperty(m => m.Name, category.Name)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCategory")
        .WithOpenApi();

        group.MapPost("/", async (Category category, BudgetWebAppContext db) =>
        {
            db.Categories.Add(category);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Category/{category.Id}", category);
        })
        .WithName("CreateCategory")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, BudgetWebAppContext db) =>
        {
            var affected = await db.Categories
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCategory")
        .WithOpenApi();
    }
}
