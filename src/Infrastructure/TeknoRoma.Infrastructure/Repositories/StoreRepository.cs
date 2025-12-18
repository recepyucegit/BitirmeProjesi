using Microsoft.EntityFrameworkCore;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Domain.Entities;
using TeknoRoma.Infrastructure.Data;

namespace TeknoRoma.Infrastructure.Repositories;

public class StoreRepository : Repository<Store>, IStoreRepository
{
    public StoreRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Store?> GetByStoreCodeAsync(string storeCode)
    {
        return await _dbSet.FirstOrDefaultAsync(s => s.StoreCode == storeCode);
    }

    public async Task<Store?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(s => s.Manager)
            .Include(s => s.Employees)
            .Include(s => s.Sales)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Store>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(s => s.Manager)
            .Include(s => s.Employees)
            .ToListAsync();
    }

    public async Task<IEnumerable<Store>> GetActiveStoresAsync()
    {
        return await _dbSet
            .Where(s => s.IsActive)
            .Include(s => s.Manager)
            .ToListAsync();
    }

    public async Task<IEnumerable<Store>> GetStoresByCityAsync(string city)
    {
        return await _dbSet
            .Where(s => s.City == city)
            .Include(s => s.Manager)
            .ToListAsync();
    }

    public async Task<IEnumerable<Store>> GetStoresByManagerAsync(int managerId)
    {
        return await _dbSet
            .Where(s => s.ManagerId == managerId)
            .ToListAsync();
    }

    public async Task<bool> StoreCodeExistsAsync(string storeCode)
    {
        return await _dbSet.AnyAsync(s => s.StoreCode == storeCode);
    }

    public async Task<int> GetEmployeeCountAsync(int storeId)
    {
        return await _context.Employees
            .Where(e => e.StoreId == storeId && !e.IsDeleted)
            .CountAsync();
    }
}
