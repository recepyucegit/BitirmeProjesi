using Microsoft.EntityFrameworkCore;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Domain.Entities;
using TeknoRoma.Infrastructure.Data;

namespace TeknoRoma.Infrastructure.Repositories;

public class DepartmentRepository : Repository<Department>, IDepartmentRepository
{
    public DepartmentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Department?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(d => d.Manager)
            .Include(d => d.Employees)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<IEnumerable<Department>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(d => d.Manager)
            .Include(d => d.Employees)
            .ToListAsync();
    }

    public async Task<Department?> GetByCodeAsync(string code)
    {
        return await _dbSet
            .Include(d => d.Manager)
            .FirstOrDefaultAsync(d => d.DepartmentCode == code);
    }

    public async Task<Department?> GetByNameAsync(string name)
    {
        return await _dbSet
            .Include(d => d.Manager)
            .FirstOrDefaultAsync(d => d.DepartmentName == name);
    }

    public async Task<IEnumerable<Department>> GetByManagerIdAsync(int managerId)
    {
        return await _dbSet
            .Include(d => d.Manager)
            .Include(d => d.Employees)
            .Where(d => d.ManagerId == managerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Department>> GetActiveDepartmentsAsync()
    {
        return await _dbSet
            .Include(d => d.Manager)
            .Where(d => d.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Department>> GetDepartmentsWithBudgetAsync(decimal minBudget, decimal maxBudget)
    {
        return await _dbSet
            .Include(d => d.Manager)
            .Where(d => d.Budget.HasValue && d.Budget >= minBudget && d.Budget <= maxBudget)
            .ToListAsync();
    }

    public async Task<int> GetTotalEmployeeCountAsync()
    {
        var departments = await _dbSet.ToListAsync();
        return departments.Sum(d => d.EmployeeCount ?? 0);
    }

    public async Task<decimal> GetTotalBudgetAsync()
    {
        var departments = await _dbSet.ToListAsync();
        return departments.Sum(d => d.Budget ?? 0);
    }
}
