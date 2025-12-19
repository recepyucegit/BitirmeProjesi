using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeknoRoma.Application.DTOs.Department;
using TeknoRoma.Application.Interfaces.Services;

namespace TeknoRoma.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAllDepartments()
    {
        var departments = await _departmentService.GetAllAsync();
        return Ok(departments);
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetActiveDepartments()
    {
        var departments = await _departmentService.GetActiveDepartmentsAsync();
        return Ok(departments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DepartmentDto>> GetDepartment(int id)
    {
        var department = await _departmentService.GetByIdAsync(id);
        if (department == null) return NotFound();
        return Ok(department);
    }

    [HttpGet("code/{code}")]
    public async Task<ActionResult<DepartmentDto>> GetDepartmentByCode(string code)
    {
        var department = await _departmentService.GetByCodeAsync(code);
        if (department == null) return NotFound();
        return Ok(department);
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<DepartmentDto>> GetDepartmentByName(string name)
    {
        var department = await _departmentService.GetByNameAsync(name);
        if (department == null) return NotFound();
        return Ok(department);
    }

    [HttpGet("manager/{managerId}")]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartmentsByManager(int managerId)
    {
        var departments = await _departmentService.GetByManagerIdAsync(managerId);
        return Ok(departments);
    }

    [HttpGet("statistics/total-employees")]
    public async Task<ActionResult<int>> GetTotalEmployeeCount()
    {
        var count = await _departmentService.GetTotalEmployeeCountAsync();
        return Ok(count);
    }

    [HttpGet("statistics/total-budget")]
    public async Task<ActionResult<decimal>> GetTotalBudget()
    {
        var budget = await _departmentService.GetTotalBudgetAsync();
        return Ok(budget);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,StoreManager")]
    public async Task<ActionResult<DepartmentDto>> CreateDepartment(CreateDepartmentDto dto)
    {
        var department = await _departmentService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, department);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,StoreManager")]
    public async Task<ActionResult<DepartmentDto>> UpdateDepartment(int id, UpdateDepartmentDto dto)
    {
        var department = await _departmentService.UpdateAsync(id, dto);
        return Ok(department);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        await _departmentService.DeleteAsync(id);
        return NoContent();
    }
}
