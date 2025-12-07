using Microsoft.EntityFrameworkCore;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Data.SeedData;

public static class ProductSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Products.AnyAsync())
            return;

        var categories = await context.Categories.ToListAsync();
        if (!categories.Any())
            throw new InvalidOperationException("Categories must be seeded first");

        var products = new List<Product>
        {
            new Product { Name = "Dell XPS 15", Barcode = "8697434567890", Price = 45000, CostPrice = 40000, StockQuantity = 15, CriticalStockLevel = 5, CategoryId = categories[0].Id, IsActive = true },
            new Product { Name = "MacBook Pro 16", Barcode = "8697434567891", Price = 65000, CostPrice = 58000, StockQuantity = 10, CriticalStockLevel = 5, CategoryId = categories[0].Id, IsActive = true },
            new Product { Name = "iPhone 15 Pro", Barcode = "8697434567893", Price = 55000, CostPrice = 48000, StockQuantity = 20, CriticalStockLevel = 10, CategoryId = categories[1].Id, IsActive = true },
            new Product { Name = "Samsung Galaxy S24", Barcode = "8697434567894", Price = 42000, CostPrice = 37000, StockQuantity = 18, CriticalStockLevel = 10, CategoryId = categories[1].Id, IsActive = true },
            new Product { Name = "Canon EOS R6", Barcode = "8697434567895", Price = 48000, CostPrice = 42000, StockQuantity = 8, CriticalStockLevel = 5, CategoryId = categories[2].Id, IsActive = true },
            new Product { Name = "Sony A7 IV", Barcode = "8697434567896", Price = 52000, CostPrice = 46000, StockQuantity = 6, CriticalStockLevel = 5, CategoryId = categories[2].Id, IsActive = true },
            new Product { Name = "AMD Ryzen 9 7950X", Barcode = "8697434567897", Price = 18000, CostPrice = 15000, StockQuantity = 12, CriticalStockLevel = 8, CategoryId = categories[3].Id, IsActive = true },
            new Product { Name = "Intel Core i9-14900K", Barcode = "8697434567898", Price = 20000, CostPrice = 17000, StockQuantity = 10, CriticalStockLevel = 8, CategoryId = categories[3].Id, IsActive = true },
            new Product { Name = "NVIDIA RTX 4080", Barcode = "8697434567899", Price = 42000, CostPrice = 37000, StockQuantity = 2, CriticalStockLevel = 5, CategoryId = categories[3].Id, IsActive = true },
            new Product { Name = "PlayStation 5", Barcode = "8697434567900", Price = 22000, CostPrice = 19000, StockQuantity = 25, CriticalStockLevel = 10, CategoryId = categories[4].Id, IsActive = true }
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }
}
