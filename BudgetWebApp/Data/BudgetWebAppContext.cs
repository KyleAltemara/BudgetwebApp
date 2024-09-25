using Microsoft.EntityFrameworkCore;
using BudgetWebApp.Models;

namespace BudgetWebApp.Data;

public class BudgetWebAppContext(DbContextOptions<BudgetWebAppContext> options) : DbContext(options)
{
    public DbSet<Transaction> Transactions { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
}
