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

    public async Task<IEnumerable<SupplierTransaction>> GetBySupplierIdAsync(int supplierId)
    {
        return await _context.SupplierTransactions
            .Where(st => st.SupplierId == supplierId && !st.IsDeleted)
            .Include(st => st.Product)
            .OrderByDescending(st => st.TransactionDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<SupplierTransaction>> GetByProductIdAsync(int productId)
    {
        return await _context.SupplierTransactions
            .Where(st => st.ProductId == productId && !st.IsDeleted)
            .Include(st => st.Supplier)
            .OrderByDescending(st => st.TransactionDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<SupplierTransaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.SupplierTransactions
            .Where(st => st.TransactionDate >= startDate && st.TransactionDate <= endDate && !st.IsDeleted)
            .Include(st => st.Supplier)
            .Include(st => st.Product)
            .OrderByDescending(st => st.TransactionDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<SupplierTransaction>> GetByTransactionTypeAsync(string transactionType)
    {
        return await _context.SupplierTransactions
            .Where(st => st.TransactionType == transactionType && !st.IsDeleted)
            .Include(st => st.Supplier)
            .Include(st => st.Product)
            .OrderByDescending(st => st.TransactionDate)
            .ToListAsync();
    }
}
