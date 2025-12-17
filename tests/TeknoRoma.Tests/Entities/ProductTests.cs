using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TeknoRoma.Domain.Entities;
using TeknoRoma.Infrastructure.Repositories;
using TeknoRoma.Tests.Helpers;

namespace TeknoRoma.Tests.Entities;

public class ProductTests
{
    [Fact]
    public async Task CreateProduct_ShouldAddToDatabase()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var categoryRepo = new CategoryRepository(context);
        var productRepo = new ProductRepository(context);

        var category = new Category { Name = "Electronics" };
        await categoryRepo.AddAsync(category);
        await context.SaveChangesAsync();

        var product = new Product
        {
            Name = "Laptop",
            Description = "Gaming laptop",
            Price = 1500.00m,
            CostPrice = 1200.00m,
            StockQuantity = 10,
            CategoryId = category.Id,
            Barcode = "123456789",
            IsActive = true
        };

        // Act
        await productRepo.AddAsync(product);
        await context.SaveChangesAsync();

        // Assert
        var savedProduct = await context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Name == "Laptop");

        savedProduct.Should().NotBeNull();
        savedProduct!.Name.Should().Be("Laptop");
        savedProduct.Price.Should().Be(1500.00m);
        savedProduct.CostPrice.Should().Be(1200.00m);
        savedProduct.StockQuantity.Should().Be(10);
        savedProduct.Category.Should().NotBeNull();
        savedProduct.Category!.Name.Should().Be("Electronics");
    }

    [Fact]
    public async Task GetAllProducts_ShouldReturnAllActiveProducts()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var categoryRepo = new CategoryRepository(context);
        var productRepo = new ProductRepository(context);

        var category = new Category { Name = "Electronics" };
        await categoryRepo.AddAsync(category);
        await context.SaveChangesAsync();

        var product1 = new Product { Name = "Product1", Price = 100, CategoryId = category.Id };
        var product2 = new Product { Name = "Product2", Price = 200, CategoryId = category.Id };
        var deletedProduct = new Product { Name = "Deleted", Price = 300, CategoryId = category.Id };

        await productRepo.AddAsync(product1);
        await productRepo.AddAsync(product2);
        await productRepo.AddAsync(deletedProduct);
        await context.SaveChangesAsync();

        // Delete the third product
        await productRepo.DeleteAsync(deletedProduct);
        await context.SaveChangesAsync();

        // Act
        var result = await productRepo.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(p => p.IsDeleted == false);
    }

    [Fact]
    public async Task GetProductById_ShouldReturnProductWithCategory()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var categoryRepo = new CategoryRepository(context);
        var productRepo = new ProductRepository(context);

        var category = new Category { Name = "Books" };
        await categoryRepo.AddAsync(category);
        await context.SaveChangesAsync();

        var product = new Product
        {
            Name = "C# Programming",
            Price = 50,
            CategoryId = category.Id
        };
        await productRepo.AddAsync(product);
        await context.SaveChangesAsync();

        // Act
        var result = await productRepo.GetByIdAsync(product.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("C# Programming");
        result.Category.Should().NotBeNull();
        result.Category!.Name.Should().Be("Books");
    }

    [Fact]
    public async Task UpdateProduct_ShouldModifyExistingProduct()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var categoryRepo = new CategoryRepository(context);
        var productRepo = new ProductRepository(context);

        var category = new Category { Name = "Electronics" };
        await categoryRepo.AddAsync(category);
        await context.SaveChangesAsync();

        var product = new Product
        {
            Name = "Old Product",
            Price = 100,
            CategoryId = category.Id
        };
        await productRepo.AddAsync(product);
        await context.SaveChangesAsync();

        // Act
        product.Name = "Updated Product";
        product.Price = 150;
        await productRepo.UpdateAsync(product);
        await context.SaveChangesAsync();

        // Assert
        var updated = await productRepo.GetByIdAsync(product.Id);
        updated.Should().NotBeNull();
        updated!.Name.Should().Be("Updated Product");
        updated.Price.Should().Be(150);
    }

    [Fact]
    public async Task DeleteProduct_ShouldSoftDelete()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var categoryRepo = new CategoryRepository(context);
        var productRepo = new ProductRepository(context);

        var category = new Category { Name = "Electronics" };
        await categoryRepo.AddAsync(category);
        await context.SaveChangesAsync();

        var product = new Product
        {
            Name = "To Delete",
            Price = 100,
            CategoryId = category.Id
        };
        await productRepo.AddAsync(product);
        await context.SaveChangesAsync();

        // Act
        await productRepo.DeleteAsync(product);
        await context.SaveChangesAsync();

        // Assert
        var deleted = await context.Products.IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == product.Id);
        deleted.Should().NotBeNull();
        deleted!.IsDeleted.Should().BeTrue();

        var activeProducts = await productRepo.GetAllAsync();
        activeProducts.Should().NotContain(p => p.Id == product.Id);
    }

    [Fact]
    public async Task GetProductsByCategoryId_ShouldReturnCorrectProducts()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var categoryRepo = new CategoryRepository(context);
        var productRepo = new ProductRepository(context);

        var category1 = new Category { Name = "Electronics" };
        var category2 = new Category { Name = "Books" };
        await categoryRepo.AddAsync(category1);
        await categoryRepo.AddAsync(category2);
        await context.SaveChangesAsync();

        var products = new List<Product>
        {
            new Product { Name = "Laptop", Price = 1000, CategoryId = category1.Id },
            new Product { Name = "Mouse", Price = 50, CategoryId = category1.Id },
            new Product { Name = "Book", Price = 30, CategoryId = category2.Id }
        };

        foreach (var product in products)
        {
            await productRepo.AddAsync(product);
        }
        await context.SaveChangesAsync();

        // Act
        var result = await productRepo.GetProductsByCategoryAsync(category1.Id);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(p => p.CategoryId == category1.Id);
    }

    [Fact]
    public async Task GetLowStockProducts_ShouldReturnProductsBelowCriticalLevel()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var categoryRepo = new CategoryRepository(context);
        var productRepo = new ProductRepository(context);

        var category = new Category { Name = "Electronics" };
        await categoryRepo.AddAsync(category);
        await context.SaveChangesAsync();

        var products = new List<Product>
        {
            new Product { Name = "Low Stock", Price = 100, CategoryId = category.Id, StockQuantity = 5, CriticalStockLevel = 10 },
            new Product { Name = "Normal Stock", Price = 100, CategoryId = category.Id, StockQuantity = 20, CriticalStockLevel = 10 }
        };

        foreach (var product in products)
        {
            await productRepo.AddAsync(product);
        }
        await context.SaveChangesAsync();

        // Act
        var result = await productRepo.GetLowStockProductsAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Low Stock");
    }
}
