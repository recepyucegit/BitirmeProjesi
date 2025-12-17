using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TeknoRoma.Domain.Entities;
using TeknoRoma.Infrastructure.Repositories;
using TeknoRoma.Tests.Helpers;

namespace TeknoRoma.Tests.Entities;

public class SupplierTransactionTests
{
    [Fact]
    public async Task CreateSupplierTransaction_ShouldAddToDatabase()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var supplierRepo = new SupplierRepository(context);
        var categoryRepo = new CategoryRepository(context);
        var productRepo = new ProductRepository(context);
        var transactionRepo = new SupplierTransactionRepository(context);

        var supplier = new Supplier { CompanyName = "Tech Supplier" };
        await supplierRepo.AddAsync(supplier);

        var category = new Category { Name = "Electronics" };
        await categoryRepo.AddAsync(category);

        await context.SaveChangesAsync();

        var product = new Product
        {
            Name = "Laptop",
            Price = 1500,
            CategoryId = category.Id,
            SupplierId = supplier.Id
        };
        await productRepo.AddAsync(product);
        await context.SaveChangesAsync();

        var transaction = new SupplierTransaction
        {
            SupplierId = supplier.Id,
            ProductId = product.Id,
            TransactionType = "Purchase",
            Amount = 12000.00m,
            Quantity = 10,
            UnitPrice = 1200.00m,
            Description = "Bulk purchase of laptops",
            TransactionDate = DateTime.UtcNow,
            InvoiceNumber = "INV-001",
            ReferenceNumber = "REF-001",
            Status = "Completed"
        };

        // Act
        await transactionRepo.AddAsync(transaction);
        await context.SaveChangesAsync();

        // Assert
        var savedTransaction = await context.SupplierTransactions
            .Include(st => st.Supplier)
            .Include(st => st.Product)
            .FirstOrDefaultAsync(st => st.InvoiceNumber == "INV-001");

        savedTransaction.Should().NotBeNull();
        savedTransaction!.TransactionType.Should().Be("Purchase");
        savedTransaction.Amount.Should().Be(12000.00m);
        savedTransaction.Quantity.Should().Be(10);
        savedTransaction.UnitPrice.Should().Be(1200.00m);
        savedTransaction.Supplier.Should().NotBeNull();
        savedTransaction.Supplier!.CompanyName.Should().Be("Tech Supplier");
        savedTransaction.Product.Should().NotBeNull();
        savedTransaction.Product!.Name.Should().Be("Laptop");
    }

    [Fact]
    public async Task GetAllTransactions_ShouldReturnAllActiveTransactions()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var supplierRepo = new SupplierRepository(context);
        var transactionRepo = new SupplierTransactionRepository(context);

        var supplier = new Supplier { CompanyName = "Tech Supplier" };
        await supplierRepo.AddAsync(supplier);
        await context.SaveChangesAsync();

        var transaction1 = new SupplierTransaction { SupplierId = supplier.Id, TransactionType = "Purchase", Amount = 1000 };
        var transaction2 = new SupplierTransaction { SupplierId = supplier.Id, TransactionType = "Payment", Amount = 500 };
        var deletedTransaction = new SupplierTransaction { SupplierId = supplier.Id, TransactionType = "Return", Amount = 200 };

        await transactionRepo.AddAsync(transaction1);
        await transactionRepo.AddAsync(transaction2);
        await transactionRepo.AddAsync(deletedTransaction);
        await context.SaveChangesAsync();

        // Delete the third transaction
        await transactionRepo.DeleteAsync(deletedTransaction);
        await context.SaveChangesAsync();

        // Act
        var result = await transactionRepo.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(t => t.IsDeleted == false);
    }

    [Fact]
    public async Task GetTransactionsBySupplierId_ShouldReturnCorrectTransactions()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var supplierRepo = new SupplierRepository(context);
        var transactionRepo = new SupplierTransactionRepository(context);

        var supplier1 = new Supplier { CompanyName = "Supplier 1" };
        var supplier2 = new Supplier { CompanyName = "Supplier 2" };
        await supplierRepo.AddAsync(supplier1);
        await supplierRepo.AddAsync(supplier2);
        await context.SaveChangesAsync();

        var transactions = new List<SupplierTransaction>
        {
            new SupplierTransaction { SupplierId = supplier1.Id, TransactionType = "Purchase", Amount = 1000 },
            new SupplierTransaction { SupplierId = supplier1.Id, TransactionType = "Payment", Amount = 500 },
            new SupplierTransaction { SupplierId = supplier2.Id, TransactionType = "Purchase", Amount = 2000 }
        };

        foreach (var transaction in transactions)
        {
            await transactionRepo.AddAsync(transaction);
        }
        await context.SaveChangesAsync();

        // Act
        var result = await transactionRepo.GetBySupplierIdAsync(supplier1.Id);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(t => t.SupplierId == supplier1.Id);
    }

    [Fact]
    public async Task GetTransactionsByProductId_ShouldReturnCorrectTransactions()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var supplierRepo = new SupplierRepository(context);
        var categoryRepo = new CategoryRepository(context);
        var productRepo = new ProductRepository(context);
        var transactionRepo = new SupplierTransactionRepository(context);

        var supplier = new Supplier { CompanyName = "Supplier" };
        await supplierRepo.AddAsync(supplier);

        var category = new Category { Name = "Electronics" };
        await categoryRepo.AddAsync(category);
        await context.SaveChangesAsync();

        var product1 = new Product { Name = "Product1", Price = 100, CategoryId = category.Id };
        var product2 = new Product { Name = "Product2", Price = 200, CategoryId = category.Id };
        await productRepo.AddAsync(product1);
        await productRepo.AddAsync(product2);
        await context.SaveChangesAsync();

        var transactions = new List<SupplierTransaction>
        {
            new SupplierTransaction { SupplierId = supplier.Id, ProductId = product1.Id, TransactionType = "Purchase", Amount = 1000 },
            new SupplierTransaction { SupplierId = supplier.Id, ProductId = product1.Id, TransactionType = "Return", Amount = 100 },
            new SupplierTransaction { SupplierId = supplier.Id, ProductId = product2.Id, TransactionType = "Purchase", Amount = 2000 }
        };

        foreach (var transaction in transactions)
        {
            await transactionRepo.AddAsync(transaction);
        }
        await context.SaveChangesAsync();

        // Act
        var result = await transactionRepo.GetByProductIdAsync(product1.Id);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(t => t.ProductId == product1.Id);
    }

    [Fact]
    public async Task GetTransactionsByDateRange_ShouldReturnCorrectTransactions()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var supplierRepo = new SupplierRepository(context);
        var transactionRepo = new SupplierTransactionRepository(context);

        var supplier = new Supplier { CompanyName = "Supplier" };
        await supplierRepo.AddAsync(supplier);
        await context.SaveChangesAsync();

        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 12, 31);

        var transactions = new List<SupplierTransaction>
        {
            new SupplierTransaction
            {
                SupplierId = supplier.Id,
                TransactionType = "Purchase",
                Amount = 1000,
                TransactionDate = new DateTime(2024, 6, 15)
            },
            new SupplierTransaction
            {
                SupplierId = supplier.Id,
                TransactionType = "Payment",
                Amount = 500,
                TransactionDate = new DateTime(2024, 7, 20)
            },
            new SupplierTransaction
            {
                SupplierId = supplier.Id,
                TransactionType = "Purchase",
                Amount = 2000,
                TransactionDate = new DateTime(2025, 1, 10)
            }
        };

        foreach (var transaction in transactions)
        {
            await transactionRepo.AddAsync(transaction);
        }
        await context.SaveChangesAsync();

        // Act
        var result = await transactionRepo.GetByDateRangeAsync(startDate, endDate);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate);
    }

    [Fact]
    public async Task GetTransactionsByType_ShouldReturnCorrectTransactions()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var supplierRepo = new SupplierRepository(context);
        var transactionRepo = new SupplierTransactionRepository(context);

        var supplier = new Supplier { CompanyName = "Supplier" };
        await supplierRepo.AddAsync(supplier);
        await context.SaveChangesAsync();

        var transactions = new List<SupplierTransaction>
        {
            new SupplierTransaction { SupplierId = supplier.Id, TransactionType = "Purchase", Amount = 1000 },
            new SupplierTransaction { SupplierId = supplier.Id, TransactionType = "Purchase", Amount = 1500 },
            new SupplierTransaction { SupplierId = supplier.Id, TransactionType = "Payment", Amount = 500 },
            new SupplierTransaction { SupplierId = supplier.Id, TransactionType = "Return", Amount = 200 }
        };

        foreach (var transaction in transactions)
        {
            await transactionRepo.AddAsync(transaction);
        }
        await context.SaveChangesAsync();

        // Act
        var result = await transactionRepo.GetByTransactionTypeAsync("Purchase");

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(t => t.TransactionType == "Purchase");
    }

    [Fact]
    public async Task UpdateTransaction_ShouldModifyExistingTransaction()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var supplierRepo = new SupplierRepository(context);
        var transactionRepo = new SupplierTransactionRepository(context);

        var supplier = new Supplier { CompanyName = "Supplier" };
        await supplierRepo.AddAsync(supplier);
        await context.SaveChangesAsync();

        var transaction = new SupplierTransaction
        {
            SupplierId = supplier.Id,
            TransactionType = "Purchase",
            Amount = 1000,
            Status = "Pending"
        };
        await transactionRepo.AddAsync(transaction);
        await context.SaveChangesAsync();

        // Act
        transaction.Amount = 1200;
        transaction.Status = "Completed";
        await transactionRepo.UpdateAsync(transaction);
        await context.SaveChangesAsync();

        // Assert
        var updated = await transactionRepo.GetByIdAsync(transaction.Id);
        updated.Should().NotBeNull();
        updated!.Amount.Should().Be(1200);
        updated.Status.Should().Be("Completed");
    }

    [Fact]
    public async Task DeleteTransaction_ShouldSoftDelete()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var supplierRepo = new SupplierRepository(context);
        var transactionRepo = new SupplierTransactionRepository(context);

        var supplier = new Supplier { CompanyName = "Supplier" };
        await supplierRepo.AddAsync(supplier);
        await context.SaveChangesAsync();

        var transaction = new SupplierTransaction
        {
            SupplierId = supplier.Id,
            TransactionType = "Purchase",
            Amount = 1000
        };
        await transactionRepo.AddAsync(transaction);
        await context.SaveChangesAsync();

        // Act
        await transactionRepo.DeleteAsync(transaction);
        await context.SaveChangesAsync();

        // Assert
        var deleted = await context.SupplierTransactions.IgnoreQueryFilters()
            .FirstOrDefaultAsync(t => t.Id == transaction.Id);
        deleted.Should().NotBeNull();
        deleted!.IsDeleted.Should().BeTrue();

        var activeTransactions = await transactionRepo.GetAllAsync();
        activeTransactions.Should().NotContain(t => t.Id == transaction.Id);
    }
}
