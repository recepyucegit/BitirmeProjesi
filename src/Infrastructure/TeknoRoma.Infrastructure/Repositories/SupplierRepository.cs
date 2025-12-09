using Microsoft.EntityFrameworkCore;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Domain.Entities;
using TeknoRoma.Infrastructure.Data;

namespace TeknoRoma.Infrastructure.Repositories;

public class SupplierRepository : Repository<Supplier>, ISupplierRepository
{
    public SupplierRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Supplier>> GetActiveSuppliersAsync()
    {
        return await _dbSet
            .Where(s => s.IsActive)
            .OrderBy(s => s.CompanyName)
            .ToListAsync();
    }

    public async Task<Supplier?> GetSupplierByTaxNumberAsync(string taxNumber)
    {
        return await _dbSet
            .FirstOrDefaultAsync(s => s.TaxNumber == taxNumber);
    }

    public async Task<Supplier?> GetSupplierWithProductsAsync(int id)
    {
        return await _dbSet
            .Include(s => s.Products)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
