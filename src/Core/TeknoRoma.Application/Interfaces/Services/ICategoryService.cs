using TeknoRoma.Application.DTOs.Category;

namespace TeknoRoma.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync();
    Task<CategoryDto?> GetCategoryByIdAsync(int id);
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
    Task<CategoryDto> UpdateCategoryAsync(UpdateCategoryDto dto);
    Task<bool> DeleteCategoryAsync(int id);
    Task<bool> CategoryExistsAsync(int id);
}
