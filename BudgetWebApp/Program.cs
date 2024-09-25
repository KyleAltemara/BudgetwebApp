using Microsoft.EntityFrameworkCore;
using BudgetWebApp.Data;
using BudgetWebApp.Controllers;

namespace BudgetWebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<BudgetWebAppContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("BudgetWebAppContext") ?? 
            throw new InvalidOperationException("Connection string 'BudgetWebAppContext' not found.")));

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            SeedData.Initialize(services);
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        };

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.MapRazorPages();

        app.MapTransactionEndpoints();
        app.MapCategoryEndpoints();

        app.Run();
    }
}
