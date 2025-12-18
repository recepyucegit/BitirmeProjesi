using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Application.Interfaces.Repositories;

public interface IDepartmentRepository : IRepository<Department>
{
    Task<Department?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Department>> GetAllWithDetailsAsync();
    Task<Department?> GetByCodeAsync(string code);
    Task<Department?> GetByNameAsync(string name);
    Task<IEnumerable<Department>> GetByManagerIdAsync(int managerId);
    Task<IEnumerable<Department>> GetActiveDepartmentsAsync();
    Task<IEnumerable<Department>> GetDepartmentsWithBudgetAsync(decimal minBudget, decimal maxBudget);
    Task<int> GetTotalEmployeeCountAsync();
    Task<decimal> GetTotalBudgetAsync();
}
