using TeknoRoma.Application.DTOs.Product;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Application.Interfaces.Services;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Barcode = product.Barcode,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CriticalStockLevel = product.CriticalStockLevel,
            IsActive = product.IsActive,
            ImageUrl = product.ImageUrl,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name,
            CreatedDate = product.CreatedDate
        };
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _unitOfWork.Products.GetAllAsync();
        return products.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductDto>> GetActiveProductsAsync()
    {
        var products = await _unitOfWork.Products.GetActiveProductsAsync();
        return products.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
    {
        var products = await _unitOfWork.Products.GetProductsByCategoryAsync(categoryId);
        return products.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductDto>> GetLowStockProductsAsync()
    {
        var products = await _unitOfWork.Products.GetLowStockProductsAsync();
        return products.Select(MapToDto);
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _unitOfWork.Products.GetProductWithCategoryAsync(id);
        return product == null ? null : MapToDto(product);
    }

    public async Task<ProductDto?> GetProductByBarcodeAsync(string barcode)
    {
        var product = await _unitOfWork.Products.GetProductByBarcodeAsync(barcode);
        return product == null ? null : MapToDto(product);
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Barcode = dto.Barcode,
            Price = dto.Price,
            CostPrice = dto.CostPrice,
            StockQuantity = dto.StockQuantity,
            CriticalStockLevel = dto.CriticalStockLevel,
            IsActive = dto.IsActive,
            ImageUrl = dto.ImageUrl,
            CategoryId = dto.CategoryId
        };

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        var created = await _unitOfWork.Products.GetProductWithCategoryAsync(product.Id);
        return MapToDto(created!);
    }

    public async Task<ProductDto> UpdateProductAsync(UpdateProductDto dto)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(dto.Id);
        if (product == null) throw new Exception("Product not found");

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Barcode = dto.Barcode;
        product.Price = dto.Price;
        product.CostPrice = dto.CostPrice;
        product.StockQuantity = dto.StockQuantity;
        product.CriticalStockLevel = dto.CriticalStockLevel;
        product.IsActive = dto.IsActive;
        product.ImageUrl = dto.ImageUrl;
        product.CategoryId = dto.CategoryId;

        await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync();

        var updated = await _unitOfWork.Products.GetProductWithCategoryAsync(product.Id);
        return MapToDto(updated!);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null) return false;

        await _unitOfWork.Products.DeleteAsync(product);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ProductExistsAsync(int id)
    {
        return await _unitOfWork.Products.AnyAsync(p => p.Id == id);
    }

    public async Task<bool> BarcodeExistsAsync(string barcode)
    {
        return await _unitOfWork.Products.AnyAsync(p => p.Barcode == barcode);
    }
}
