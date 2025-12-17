using TeknoRoma.Application.DTOs.Employee;

namespace TeknoRoma.Application.Interfaces.Services;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
    Task<IEnumerable<EmployeeDto>> GetActiveEmployeesAsync();
    Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
    Task<EmployeeDto?> GetEmployeeByUsernameAsync(string username);
    Task<EmployeeDto?> GetEmployeeByIdentityNumberAsync(string identityNumber);
    Task<IEnumerable<EmployeeDto>> GetEmployeesByRoleAsync(string role);
    Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto);
    Task<EmployeeDto> UpdateEmployeeAsync(UpdateEmployeeDto dto);
    Task<bool> DeleteEmployeeAsync(int id);
    Task<bool> EmployeeExistsAsync(int id);
    Task<bool> UsernameExistsAsync(string username);
    Task<bool> IdentityNumberExistsAsync(string identityNumber);
}
