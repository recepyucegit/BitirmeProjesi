using Microsoft.AspNetCore.Mvc;
using TeknoRoma.Application.DTOs.Category;
using TeknoRoma.Application.Interfaces.Services;

namespace TeknoRoma.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetActiveCategories()
    {
        var categories = await _categoryService.GetActiveCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null) return NotFound();
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto dto)
    {
        var category = await _categoryService.CreateCategoryAsync(dto);
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, UpdateCategoryDto dto)
    {
        if (id != dto.Id) return BadRequest();
        var category = await _categoryService.UpdateCategoryAsync(dto);
        return Ok(category);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var result = await _categoryService.DeleteCategoryAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpGet("{id}/exists")]
    public async Task<ActionResult<bool>> CategoryExists(int id)
    {
        var exists = await _categoryService.CategoryExistsAsync(id);
        return Ok(exists);
    }
}
