using TeknoRoma.Application.DTOs.Department;

namespace TeknoRoma.Application.Interfaces.Services;

public interface IDepartmentService
{
    Task<DepartmentDto?> GetByIdAsync(int id);
    Task<IEnumerable<DepartmentDto>> GetAllAsync();
    Task<DepartmentDto?> GetByCodeAsync(string code);
    Task<DepartmentDto?> GetByNameAsync(string name);
    Task<IEnumerable<DepartmentDto>> GetByManagerIdAsync(int managerId);
    Task<IEnumerable<DepartmentDto>> GetActiveDepartmentsAsync();
    Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto);
    Task<DepartmentDto> UpdateAsync(int id, UpdateDepartmentDto dto);
    Task DeleteAsync(int id);
    Task<int> GetTotalEmployeeCountAsync();
    Task<decimal> GetTotalBudgetAsync();
}
