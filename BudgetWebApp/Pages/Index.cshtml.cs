using BudgetWebApp.Data;
using BudgetWebApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BudgetWebApp.Pages;

public class IndexModel(BudgetWebAppContext context, ILogger<IndexModel> logger) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;

    private readonly BudgetWebAppContext _context = context;

    public IList<Transaction> Transactions { get; set; } = default!;

    public IList<Category> Categories { get; set; } = default!;

    public async void OnGet()
    {
        var transactions = from t in _context.Transactions
                           select t;
        Transactions = await transactions.ToListAsync();

        var categories = from c in _context.Categories
                         select c;
        Categories = await categories.ToListAsync();
    }
}
