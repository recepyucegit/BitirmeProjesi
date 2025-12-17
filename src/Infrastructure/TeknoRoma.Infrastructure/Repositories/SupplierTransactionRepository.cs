using Microsoft.EntityFrameworkCore;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Domain.Entities;
using TeknoRoma.Infrastructure.Data;

namespace TeknoRoma.Infrastructure.Repositories;

public class SupplierTransactionRepository : Repository<SupplierTransaction>, ISupplierTransactionRepository
{
    public SupplierTransactionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<SupplierTransaction?> GetTransactionWithDetailsAsync(int id)
    {
        return await _context.SupplierTransactions
            .Include(st => st.Supplier)
            .Include(st => st.Employee)
            .Include(st => st.Details)
                .ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(st => st.Id == id && !st.IsDeleted);
    }

    public async Task<IEnumerable<SupplierTransaction>> GetTransactionsWithDetailsAsync()
    {
        return await _context.SupplierTransactions
            .Include(st => st.Supplier)
            .Include(st => st.Employee)
            .Include(st => st.Details)
                .ThenInclude(d => d.Product)
            .Where(st => !st.IsDeleted)
            .OrderByDescending(st => st.OrderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<SupplierTransaction>> GetTransactionsBySupplierIdAsync(int supplierId)
    {
        return await _context.SupplierTransactions
            .Include(st => st.Supplier)
            .Include(st => st.Employee)
            .Include(st => st.Details)
            .Where(st => st.SupplierId == supplierId && !st.IsDeleted)
            .OrderByDescending(st => st.OrderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<SupplierTransaction>> GetTransactionsByEmployeeIdAsync(int employeeId)
    {
        return await _context.SupplierTransactions
            .Include(st => st.Supplier)
            .Include(st => st.Employee)
            .Include(st => st.Details)
            .Where(st => st.EmployeeId == employeeId && !st.IsDeleted)
            .OrderByDescending(st => st.OrderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<SupplierTransaction>> GetTransactionsByStatusAsync(string status)
    {
        return await _context.SupplierTransactions
            .Include(st => st.Supplier)
            .Include(st => st.Employee)
            .Include(st => st.Details)
            .Where(st => st.Status == status && !st.IsDeleted)
            .OrderByDescending(st => st.OrderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<SupplierTransaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.SupplierTransactions
            .Include(st => st.Supplier)
            .Include(st => st.Employee)
            .Include(st => st.Details)
            .Where(st => st.OrderDate >= startDate && st.OrderDate <= endDate && !st.IsDeleted)
            .OrderByDescending(st => st.OrderDate)
            .ToListAsync();
    }
}
