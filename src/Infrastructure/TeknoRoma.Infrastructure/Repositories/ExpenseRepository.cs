using Microsoft.EntityFrameworkCore;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Domain.Entities;
using TeknoRoma.Infrastructure.Data;

namespace TeknoRoma.Infrastructure.Repositories;

public class ExpenseRepository : Repository<Expense>, IExpenseRepository
{
    public ExpenseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Expense?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(e => e.Employee)
            .Include(e => e.Store)
            .Include(e => e.Approver)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Expense>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(e => e.Employee)
            .Include(e => e.Store)
            .Include(e => e.Approver)
            .OrderByDescending(e => e.ExpenseDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetByStoreIdAsync(int storeId)
    {
        return await _dbSet
            .Where(e => e.StoreId == storeId)
            .Include(e => e.Employee)
            .Include(e => e.Approver)
            .OrderByDescending(e => e.ExpenseDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetByEmployeeIdAsync(int employeeId)
    {
        return await _dbSet
            .Where(e => e.EmployeeId == employeeId)
            .Include(e => e.Store)
            .OrderByDescending(e => e.ExpenseDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetByStatusAsync(string status)
    {
        return await _dbSet
            .Where(e => e.Status == status)
            .Include(e => e.Employee)
            .Include(e => e.Store)
            .OrderByDescending(e => e.ExpenseDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetByExpenseTypeAsync(string expenseType)
    {
        return await _dbSet
            .Where(e => e.ExpenseType == expenseType)
            .Include(e => e.Employee)
            .Include(e => e.Store)
            .OrderByDescending(e => e.ExpenseDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetByCategoryAsync(string category)
    {
        return await _dbSet
            .Where(e => e.Category == category)
            .Include(e => e.Employee)
            .Include(e => e.Store)
            .OrderByDescending(e => e.ExpenseDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(e => e.ExpenseDate >= startDate && e.ExpenseDate <= endDate)
            .Include(e => e.Employee)
            .Include(e => e.Store)
            .OrderByDescending(e => e.ExpenseDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetPendingExpensesAsync()
    {
        return await _dbSet
            .Where(e => e.Status == "Pending")
            .Include(e => e.Employee)
            .Include(e => e.Store)
            .OrderBy(e => e.ExpenseDate)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalExpensesByStoreAsync(int storeId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _dbSet.Where(e => e.StoreId == storeId && e.Status == "Paid");

        if (startDate.HasValue)
            query = query.Where(e => e.ExpenseDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(e => e.ExpenseDate <= endDate.Value);

        return await query.SumAsync(e => e.AmountInTL);
    }

    public async Task<decimal> GetTotalExpensesByCategoryAsync(string category, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _dbSet.Where(e => e.Category == category && e.Status == "Paid");

        if (startDate.HasValue)
            query = query.Where(e => e.ExpenseDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(e => e.ExpenseDate <= endDate.Value);

        return await query.SumAsync(e => e.AmountInTL);
    }
}
