using Microsoft.EntityFrameworkCore;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Domain.Entities;
using TeknoRoma.Infrastructure.Data;

namespace TeknoRoma.Infrastructure.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Customer>> GetActiveCustomersAsync()
    {
        return await _context.Customers
            .Where(c => c.IsActive && !c.IsDeleted)
            .OrderBy(c => c.FirstName)
            .ThenBy(c => c.LastName)
            .ToListAsync();
    }

    public async Task<Customer?> GetCustomerByIdentityNumberAsync(string identityNumber)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.IdentityNumber == identityNumber && !c.IsDeleted);
    }

    public async Task<IEnumerable<Customer>> GetCustomersByTypeAsync(string customerType)
    {
        return await _context.Customers
            .Where(c => c.CustomerType == customerType && !c.IsDeleted)
            .OrderBy(c => c.FirstName)
            .ThenBy(c => c.LastName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Customer>> GetCustomersByCityAsync(string city)
    {
        return await _context.Customers
            .Where(c => c.City == city && !c.IsDeleted)
            .OrderBy(c => c.FirstName)
            .ThenBy(c => c.LastName)
            .ToListAsync();
    }

    public async Task<bool> IdentityNumberExistsAsync(string identityNumber)
    {
        return await _context.Customers
            .AnyAsync(c => c.IdentityNumber == identityNumber && !c.IsDeleted);
    }
}
