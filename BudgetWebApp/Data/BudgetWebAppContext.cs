using Microsoft.EntityFrameworkCore;
using BudgetWebApp.Models;

namespace BudgetWebApp.Data;

public class BudgetWebAppContext(DbContextOptions<BudgetWebAppContext> options) : DbContext(options)
{
    public DbSet<Transaction> Transactions { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>().ToTable("Transaction")
            .HasOne(t => t.Category)
            .WithMany(c => c.Transactions)
            .HasForeignKey(t => t.CategoryId);

        modelBuilder.Entity<Category>().ToTable("Category")
                .HasIndex(c => c.Name)
                .IsUnique(); // Enforce uniqueness on the Name property

    }
}
