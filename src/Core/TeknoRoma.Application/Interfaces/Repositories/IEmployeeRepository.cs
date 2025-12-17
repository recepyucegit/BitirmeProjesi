using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Application.Interfaces.Repositories;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<IEnumerable<Employee>> GetActiveEmployeesAsync();
    Task<Employee?> GetEmployeeByUsernameAsync(string username);
    Task<Employee?> GetEmployeeByIdentityNumberAsync(string identityNumber);
    Task<IEnumerable<Employee>> GetEmployeesByRoleAsync(string role);
    Task<bool> UsernameExistsAsync(string username);
    Task<bool> IdentityNumberExistsAsync(string identityNumber);
}
