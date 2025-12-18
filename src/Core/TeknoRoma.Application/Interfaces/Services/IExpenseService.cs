using TeknoRoma.Application.DTOs.Expense;

namespace TeknoRoma.Application.Interfaces.Services;

public interface IExpenseService
{
    Task<ExpenseDto?> GetByIdAsync(int id);
    Task<IEnumerable<ExpenseDto>> GetAllAsync();
    Task<IEnumerable<ExpenseDto>> GetByStoreIdAsync(int storeId);
    Task<IEnumerable<ExpenseDto>> GetByStatusAsync(string status);
    Task<IEnumerable<ExpenseDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<ExpenseDto>> GetPendingExpensesAsync();
    Task<ExpenseDto> CreateAsync(CreateExpenseDto dto);
    Task<ExpenseDto> UpdateAsync(int id, UpdateExpenseDto dto);
    Task<ExpenseDto> ApproveExpenseAsync(int id, ApproveExpenseDto dto);
    Task DeleteAsync(int id);
    Task<decimal> GetTotalExpensesByStoreAsync(int storeId, DateTime? startDate = null, DateTime? endDate = null);
    Task<decimal> GetTotalExpensesByCategoryAsync(string category, DateTime? startDate = null, DateTime? endDate = null);
}
