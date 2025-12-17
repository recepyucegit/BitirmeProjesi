using TeknoRoma.Application.DTOs.SupplierTransaction;

namespace TeknoRoma.Application.Interfaces.Services;

public interface ISupplierTransactionService
{
    Task<IEnumerable<SupplierTransactionDto>> GetAllAsync();
    Task<SupplierTransactionDto?> GetByIdAsync(int id);
    Task<IEnumerable<SupplierTransactionDto>> GetBySupplierIdAsync(int supplierId);
    Task<IEnumerable<SupplierTransactionDto>> GetByProductIdAsync(int productId);
    Task<IEnumerable<SupplierTransactionDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<SupplierTransactionDto>> GetByTransactionTypeAsync(string transactionType);
    Task<SupplierTransactionDto> CreateAsync(CreateSupplierTransactionDto createDto);
    Task<SupplierTransactionDto> UpdateAsync(UpdateSupplierTransactionDto updateDto);
    Task<bool> DeleteAsync(int id);
}
