using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TeknoRoma.Domain.Entities;
using TeknoRoma.Infrastructure.Repositories;
using TeknoRoma.Tests.Helpers;

namespace TeknoRoma.Tests.Entities;

public class CategoryTests
{
    [Fact]
    public async Task CreateCategory_ShouldAddToDatabase()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var repository = new CategoryRepository(context);
        var category = new Category
        {
            Name = "Electronics",
            Description = "Electronic products",
            IsActive = true
        };

        // Act
        await repository.AddAsync(category);
        await context.SaveChangesAsync();

        // Assert
        var savedCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Electronics");
        savedCategory.Should().NotBeNull();
        savedCategory!.Name.Should().Be("Electronics");
        savedCategory.Description.Should().Be("Electronic products");
        savedCategory.IsActive.Should().BeTrue();
        savedCategory.CreatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task GetAllCategories_ShouldReturnAllActiveCategories()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var repository = new CategoryRepository(context);

        var activeCategory1 = new Category { Name = "Electronics", IsActive = true };
        var activeCategory2 = new Category { Name = "Clothing", IsActive = true };
        var deletedCategory = new Category { Name = "Deleted", IsActive = false };

        await repository.AddAsync(activeCategory1);
        await repository.AddAsync(activeCategory2);
        await repository.AddAsync(deletedCategory);
        await context.SaveChangesAsync();

        // Delete the third category
        await repository.DeleteAsync(deletedCategory);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(c => c.IsDeleted == false);
    }

    [Fact]
    public async Task GetCategoryById_ShouldReturnCorrectCategory()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var repository = new CategoryRepository(context);

        var category = new Category { Name = "Books", Description = "Book category" };
        await repository.AddAsync(category);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(category.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Books");
        result.Description.Should().Be("Book category");
    }

    [Fact]
    public async Task UpdateCategory_ShouldModifyExistingCategory()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var repository = new CategoryRepository(context);

        var category = new Category { Name = "Old Name", Description = "Old Description" };
        await repository.AddAsync(category);
        await context.SaveChangesAsync();

        // Act
        category.Name = "New Name";
        category.Description = "New Description";
        await repository.UpdateAsync(category);
        await context.SaveChangesAsync();

        // Assert
        var updated = await repository.GetByIdAsync(category.Id);
        updated.Should().NotBeNull();
        updated!.Name.Should().Be("New Name");
        updated.Description.Should().Be("New Description");
        updated.UpdatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task DeleteCategory_ShouldSoftDelete()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var repository = new CategoryRepository(context);

        var category = new Category { Name = "To Delete" };
        await repository.AddAsync(category);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(category);
        await context.SaveChangesAsync();

        // Assert
        var deleted = await context.Categories.IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == category.Id);
        deleted.Should().NotBeNull();
        deleted!.IsDeleted.Should().BeTrue();
        deleted.UpdatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));

        var activeCategories = await repository.GetAllAsync();
        activeCategories.Should().NotContain(c => c.Id == category.Id);
    }

    [Fact]
    public async Task GetActiveCategories_ShouldReturnOnlyActiveOnes()
    {
        // Arrange
        using var context = DbContextHelper.CreateInMemoryContext();
        var repository = new CategoryRepository(context);

        var activeCategory = new Category { Name = "Active", IsActive = true };
        var inactiveCategory = new Category { Name = "Inactive", IsActive = false };

        await repository.AddAsync(activeCategory);
        await repository.AddAsync(inactiveCategory);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetActiveCategoriesAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Active");
    }
}
