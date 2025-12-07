using TeknoRoma.Application.DTOs.Product;

namespace TeknoRoma.Application.Interfaces.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<IEnumerable<ProductDto>> GetActiveProductsAsync();
    Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId);
    Task<IEnumerable<ProductDto>> GetLowStockProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<ProductDto?> GetProductByBarcodeAsync(string barcode);
    Task<ProductDto> CreateProductAsync(CreateProductDto dto);
    Task<ProductDto> UpdateProductAsync(UpdateProductDto dto);
    Task<bool> DeleteProductAsync(int id);
    Task<bool> ProductExistsAsync(int id);
    Task<bool> BarcodeExistsAsync(string barcode);
}
