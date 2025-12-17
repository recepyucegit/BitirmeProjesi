using Microsoft.EntityFrameworkCore;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Domain.Entities;
using TeknoRoma.Infrastructure.Data;

namespace TeknoRoma.Infrastructure.Repositories;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
    {
        return await _dbSet
            .Where(e => e.IsActive)
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .ToListAsync();
    }

    public async Task<Employee?> GetEmployeeByUsernameAsync(string username)
    {
        return await _dbSet
            .FirstOrDefaultAsync(e => e.Username == username);
    }

    public async Task<Employee?> GetEmployeeByIdentityNumberAsync(string identityNumber)
    {
        return await _dbSet
            .FirstOrDefaultAsync(e => e.IdentityNumber == identityNumber);
    }

    public async Task<IEnumerable<Employee>> GetEmployeesByRoleAsync(string role)
    {
        return await _dbSet
            .Where(e => e.Role == role && e.IsActive)
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .ToListAsync();
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _dbSet.AnyAsync(e => e.Username == username);
    }

    public async Task<bool> IdentityNumberExistsAsync(string identityNumber)
    {
        return await _dbSet.AnyAsync(e => e.IdentityNumber == identityNumber);
    }
}
