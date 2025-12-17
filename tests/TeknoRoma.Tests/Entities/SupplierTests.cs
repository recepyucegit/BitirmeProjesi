using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TeknoRoma.Domain.Entities;
using TeknoRoma.Infrastructure.Repositories;
using TeknoRoma.Tests.Helpers;

namespace TeknoRoma.Tests.Entities;

public class SupplierTests
{
    [Fact]
    public async Task CreateSupplier_ShouldAddToDatabase()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var repository = new SupplierRepository(context);

        var supplier = new Supplier
        {
            CompanyName = "Tech Supplies Inc",
            ContactName = "John Doe",
            ContactTitle = "Sales Manager",
            Email = "john@techsupplies.com",
            Phone = "+1234567890",
            Address = "123 Tech Street",
            City = "Tech City",
            Country = "USA",
            PostalCode = "12345",
            TaxNumber = "TAX123456",
            IsActive = true
        };

        // Act
        await repository.AddAsync(supplier);
        await context.SaveChangesAsync();

        // Assert
        var savedSupplier = await context.Suppliers
            .FirstOrDefaultAsync(s => s.CompanyName == "Tech Supplies Inc");

        savedSupplier.Should().NotBeNull();
        savedSupplier!.CompanyName.Should().Be("Tech Supplies Inc");
        savedSupplier.ContactName.Should().Be("John Doe");
        savedSupplier.Email.Should().Be("john@techsupplies.com");
        savedSupplier.Phone.Should().Be("+1234567890");
        savedSupplier.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task GetAllSuppliers_ShouldReturnAllActiveSuppliers()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var repository = new SupplierRepository(context);

        var supplier1 = new Supplier { CompanyName = "Supplier1", IsActive = true };
        var supplier2 = new Supplier { CompanyName = "Supplier2", IsActive = true };
        var deletedSupplier = new Supplier { CompanyName = "Deleted", IsActive = false };

        await repository.AddAsync(supplier1);
        await repository.AddAsync(supplier2);
        await repository.AddAsync(deletedSupplier);
        await context.SaveChangesAsync();

        // Delete the third supplier
        await repository.DeleteAsync(deletedSupplier);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(s => s.IsDeleted == false);
    }

    [Fact]
    public async Task GetSupplierById_ShouldReturnCorrectSupplier()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var repository = new SupplierRepository(context);

        var supplier = new Supplier
        {
            CompanyName = "ABC Corp",
            Email = "contact@abc.com"
        };
        await repository.AddAsync(supplier);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(supplier.Id);

        // Assert
        result.Should().NotBeNull();
        result!.CompanyName.Should().Be("ABC Corp");
        result.Email.Should().Be("contact@abc.com");
    }

    [Fact]
    public async Task UpdateSupplier_ShouldModifyExistingSupplier()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var repository = new SupplierRepository(context);

        var supplier = new Supplier
        {
            CompanyName = "Old Company",
            Email = "old@email.com"
        };
        await repository.AddAsync(supplier);
        await context.SaveChangesAsync();

        // Act
        supplier.CompanyName = "New Company";
        supplier.Email = "new@email.com";
        await repository.UpdateAsync(supplier);
        await context.SaveChangesAsync();

        // Assert
        var updated = await repository.GetByIdAsync(supplier.Id);
        updated.Should().NotBeNull();
        updated!.CompanyName.Should().Be("New Company");
        updated.Email.Should().Be("new@email.com");
        updated.UpdatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task DeleteSupplier_ShouldSoftDelete()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var repository = new SupplierRepository(context);

        var supplier = new Supplier { CompanyName = "To Delete" };
        await repository.AddAsync(supplier);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(supplier);
        await context.SaveChangesAsync();

        // Assert
        var deleted = await context.Suppliers.IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.Id == supplier.Id);
        deleted.Should().NotBeNull();
        deleted!.IsDeleted.Should().BeTrue();

        var activeSuppliers = await repository.GetAllAsync();
        activeSuppliers.Should().NotContain(s => s.Id == supplier.Id);
    }

    [Fact]
    public async Task GetActiveSuppliers_ShouldReturnOnlyActiveOnes()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var repository = new SupplierRepository(context);

        var activeSupplier = new Supplier { CompanyName = "Active Supplier", IsActive = true };
        var inactiveSupplier = new Supplier { CompanyName = "Inactive Supplier", IsActive = false };

        await repository.AddAsync(activeSupplier);
        await repository.AddAsync(inactiveSupplier);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetActiveSuppliersAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().CompanyName.Should().Be("Active Supplier");
    }

    [Fact]
    public async Task GetSupplierWithProducts_ShouldIncludeProducts()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var supplierRepo = new SupplierRepository(context);
        var categoryRepo = new CategoryRepository(context);
        var productRepo = new ProductRepository(context);

        var category = new Category { Name = "Electronics" };
        await categoryRepo.AddAsync(category);
        await context.SaveChangesAsync();

        var supplier = new Supplier { CompanyName = "Tech Supplier" };
        await supplierRepo.AddAsync(supplier);
        await context.SaveChangesAsync();

        var products = new List<Product>
        {
            new Product { Name = "Product1", Price = 100, CategoryId = category.Id, SupplierId = supplier.Id },
            new Product { Name = "Product2", Price = 200, CategoryId = category.Id, SupplierId = supplier.Id }
        };

        foreach (var product in products)
        {
            await productRepo.AddAsync(product);
        }
        await context.SaveChangesAsync();

        // Act
        var result = await context.Suppliers
            .Include(s => s.Products)
            .FirstOrDefaultAsync(s => s.Id == supplier.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Products.Should().HaveCount(2);
    }
}
