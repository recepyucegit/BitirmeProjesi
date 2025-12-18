using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Application.Interfaces.Repositories;

public interface IExpenseRepository : IRepository<Expense>
{
    Task<Expense?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Expense>> GetAllWithDetailsAsync();
    Task<IEnumerable<Expense>> GetByStoreIdAsync(int storeId);
    Task<IEnumerable<Expense>> GetByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<Expense>> GetByStatusAsync(string status);
    Task<IEnumerable<Expense>> GetByExpenseTypeAsync(string expenseType);
    Task<IEnumerable<Expense>> GetByCategoryAsync(string category);
    Task<IEnumerable<Expense>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Expense>> GetPendingExpensesAsync();
    Task<decimal> GetTotalExpensesByStoreAsync(int storeId, DateTime? startDate = null, DateTime? endDate = null);
    Task<decimal> GetTotalExpensesByCategoryAsync(string category, DateTime? startDate = null, DateTime? endDate = null);
}
