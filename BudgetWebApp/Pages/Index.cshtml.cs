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

    public SelectList CategorySelectList { get; set; } = default!;

    [BindProperty]
    public Transaction AddTransaction { get; set; } = default!;

    [BindProperty]
    public Transaction EditTransaction { get; set; } = default!;

    [BindProperty]
    public string? NewCategoryName_Add { get; set; }

    [BindProperty]
    public string? NewCategoryName_Edit { get; set; }

    [BindProperty(SupportsGet = true)]
    public string SortOrder { get; set; } = "date_desc";

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? CategoryFilter { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateOnly? StartDate { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateOnly? EndDate { get; set; }

    public async Task OnGetAsync()
    {

        var transactions = from m in _context.Transactions
                           select m;

        if (!string.IsNullOrEmpty(SearchString))
        {
            transactions = transactions.Where(s => s.Name.Contains(SearchString));
        }

        if (CategoryFilter != null)
        {
            transactions = transactions.Where(s => s.CategoryId == CategoryFilter);
        }

        if (StartDate != null)
        {
            transactions = transactions.Where(s => s.Date >= StartDate);
        }

        if (EndDate != null)
        {
            transactions = transactions.Where(s => s.Date <= EndDate);
        }

        transactions = SortOrder switch
        {
            "category_asc" => transactions.OrderBy(s => s.Category.Name),
            "category_desc" => transactions.OrderByDescending(s => s.Category.Name),
            "amount_asc" => transactions.OrderBy(s => s.Amount),
            "amount_desc" => transactions.OrderByDescending(s => s.Amount),
            "date_asc" => transactions.OrderBy(s => s.Date),
            "date_desc" => transactions.OrderByDescending(s => s.Date),
            "name_asc" => transactions.OrderBy(s => s.Name),
            "name_desc" => transactions.OrderByDescending(s => s.Name),
            _ => throw new Exception()
        };

        Transactions = await transactions.ToListAsync();
        Categories = await _context.Categories.ToListAsync();
        var tempCategories = new List<Category>(Categories)
        {
            new() { Id = -1, Name = "Add new category" }
        };

        CategorySelectList = new SelectList(tempCategories, "Id", "Name");

        _logger.LogInformation("Transactions and Categories loaded");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("SortOrder");
        if (Request.Form["_method"] == "PUT")
        {
            return await OnPutAsync();
        }

        ModelState.Remove("AddTransaction.Id");
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (AddTransaction.CategoryId == -1 && !string.IsNullOrEmpty(NewCategoryName_Add))
        {
            var newCategory = new Category { Name = NewCategoryName_Add };
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();
            newCategory = await _context.Categories.FirstAsync(c => c.Name == NewCategoryName_Add);
            AddTransaction.CategoryId = newCategory.Id;
        }

        var transactionToAdd = new Transaction
        {
            Name = AddTransaction.Name,
            Category = await _context.Categories.FindAsync(AddTransaction.CategoryId),
            Amount = AddTransaction.Amount,
            Date = AddTransaction.Date
        };

        _context.Transactions.Add(transactionToAdd);
        await _context.SaveChangesAsync();
        _logger.LogInformation($"Transaction added: Category: {transactionToAdd.Category?.Name}, Amount: {transactionToAdd.Amount}, Date: {transactionToAdd.Date}");
        AddTransaction = transactionToAdd;

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPutAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (EditTransaction.CategoryId == -1 && !string.IsNullOrEmpty(NewCategoryName_Edit))
        {
            var newCategory = new Category { Name = NewCategoryName_Edit };
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();
            newCategory = await _context.Categories.FirstAsync(c => c.Name == NewCategoryName_Edit);
            EditTransaction.CategoryId = newCategory.Id;
        }

        var transactionToUpdate = await _context.Transactions.FindAsync(EditTransaction.Id);

        if (transactionToUpdate == null)
        {
            return NotFound();
        }

        transactionToUpdate.Name = EditTransaction.Name;
        transactionToUpdate.Category = await _context.Categories.FindAsync(EditTransaction.CategoryId);
        transactionToUpdate.Amount = EditTransaction.Amount;
        transactionToUpdate.Date = EditTransaction.Date;
        _logger.LogInformation($"Transaction updated: Name: {transactionToUpdate.Name}, Category: {transactionToUpdate.Category?.Name}, Amount: {transactionToUpdate.Amount}, Date: {transactionToUpdate.Date}");
        await _context.SaveChangesAsync();

        return RedirectToPage();
    }
}
