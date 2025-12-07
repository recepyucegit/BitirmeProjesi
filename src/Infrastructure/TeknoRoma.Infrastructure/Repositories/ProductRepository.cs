using Microsoft.EntityFrameworkCore;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Domain.Entities;
using TeknoRoma.Infrastructure.Data;

namespace TeknoRoma.Infrastructure.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        return await _dbSet
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        return await _dbSet
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Product?> GetProductByBarcodeAsync(string barcode)
    {
        return await _dbSet
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Barcode == barcode);
    }

    public async Task<IEnumerable<Product>> GetLowStockProductsAsync()
    {
        return await _dbSet
            .Include(p => p.Category)
            .Where(p => p.StockQuantity <= p.CriticalStockLevel)
            .OrderBy(p => p.StockQuantity)
            .ToListAsync();
    }

    public async Task<Product?> GetProductWithCategoryAsync(int id)
    {
        return await _dbSet
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
