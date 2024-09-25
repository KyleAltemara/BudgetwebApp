using BudgetWebApp.Data;
using BudgetWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BudgetWebApp.Pages;

public class IndexModel(BudgetWebAppContext context, ILogger<IndexModel> logger) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;

    private readonly BudgetWebAppContext _context = context;

    public IList<Transaction> Transactions { get; set; } = default!;

    public IList<Category> Categories { get; set; } = default!;

    [BindProperty]
    public Transaction Transaction { get; set; } = default!;

    [BindProperty]
    public string? NewCategoryName { get; set; }

    public SelectList CategorySelectList { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Transactions = await _context.Transactions.Include(t => t.Category).ToListAsync();
        var Categories = await _context.Categories.ToListAsync();
        var tempCategories = new List<Category>(Categories)
        {
            new() { Id = -1, Name = "Add new category" }
        };

        CategorySelectList = new SelectList(tempCategories, "Id", "Name");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Transaction.CategoryId == -1 && !string.IsNullOrEmpty(NewCategoryName))
        {
            var newCategory = new Category { Name = NewCategoryName };
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();
            newCategory = await _context.Categories.FirstAsync(c => c.Name == NewCategoryName);
            Transaction.CategoryId = newCategory.Id;
        }

        var transactionToUpdate = await _context.Transactions.FindAsync(Transaction.Id);

        if (transactionToUpdate == null)
        {
            return NotFound();
        }

        transactionToUpdate.Category = await _context.Categories.FindAsync(Transaction.CategoryId);
        transactionToUpdate.Amount = Transaction.Amount;
        transactionToUpdate.Date = Transaction.Date;

        await _context.SaveChangesAsync();

        return RedirectToPage();
    }
}
