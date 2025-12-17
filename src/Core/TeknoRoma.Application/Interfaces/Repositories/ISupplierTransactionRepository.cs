using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Application.Interfaces.Repositories;

public interface ISupplierTransactionRepository : IRepository<SupplierTransaction>
{
    Task<IEnumerable<SupplierTransaction>> GetBySupplierIdAsync(int supplierId);
    Task<IEnumerable<SupplierTransaction>> GetByProductIdAsync(int productId);
    Task<IEnumerable<SupplierTransaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<SupplierTransaction>> GetByTransactionTypeAsync(string transactionType);
}
