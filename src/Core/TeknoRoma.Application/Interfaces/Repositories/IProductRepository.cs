using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Application.Interfaces.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetActiveProductsAsync();
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
    Task<Product?> GetProductByBarcodeAsync(string barcode);
    Task<IEnumerable<Product>> GetLowStockProductsAsync();
    Task<Product?> GetProductWithCategoryAsync(int id);
}
