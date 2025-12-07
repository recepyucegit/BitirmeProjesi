using TeknoRoma.Application.DTOs.Category;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Application.Interfaces.Services;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
        return categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            IsActive = c.IsActive,
            CreatedDate = c.CreatedDate
        });
    }

    public async Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync()
    {
        var categories = await _unitOfWork.Categories.GetActiveCategoriesAsync();
        return categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            IsActive = c.IsActive,
            CreatedDate = c.CreatedDate
        });
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null) return null;

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive,
            CreatedDate = category.CreatedDate
        };
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            IsActive = dto.IsActive
        };

        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive,
            CreatedDate = category.CreatedDate
        };
    }

    public async Task<CategoryDto> UpdateCategoryAsync(UpdateCategoryDto dto)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(dto.Id);
        if (category == null) throw new Exception("Category not found");

        category.Name = dto.Name;
        category.Description = dto.Description;
        category.IsActive = dto.IsActive;

        await _unitOfWork.Categories.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive,
            CreatedDate = category.CreatedDate
        };
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null) return false;

        await _unitOfWork.Categories.DeleteAsync(category);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CategoryExistsAsync(int id)
    {
        return await _unitOfWork.Categories.AnyAsync(c => c.Id == id);
    }
}
