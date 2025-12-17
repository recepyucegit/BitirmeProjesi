using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Application.Interfaces.Repositories;

public interface ISupplierTransactionRepository : IRepository<SupplierTransaction>
{
    Task<SupplierTransaction?> GetTransactionWithDetailsAsync(int id);
    Task<IEnumerable<SupplierTransaction>> GetTransactionsBySupplierIdAsync(int supplierId);
    Task<IEnumerable<SupplierTransaction>> GetTransactionsByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<SupplierTransaction>> GetTransactionsByStatusAsync(string status);
    Task<IEnumerable<SupplierTransaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<SupplierTransaction>> GetTransactionsWithDetailsAsync();
}
