using TeknoRoma.Application.DTOs.SupplierTransaction;

namespace TeknoRoma.Application.Interfaces.Services;

public interface ISupplierTransactionService
{
    Task<IEnumerable<SupplierTransactionDto>> GetAllTransactionsAsync();
    Task<IEnumerable<SupplierTransactionDto>> GetTransactionsWithDetailsAsync();
    Task<SupplierTransactionDto?> GetTransactionByIdAsync(int id);
    Task<SupplierTransactionDto?> GetTransactionWithDetailsAsync(int id);
    Task<IEnumerable<SupplierTransactionDto>> GetTransactionsBySupplierIdAsync(int supplierId);
    Task<IEnumerable<SupplierTransactionDto>> GetTransactionsByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<SupplierTransactionDto>> GetTransactionsByStatusAsync(string status);
    Task<IEnumerable<SupplierTransactionDto>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<SupplierTransactionDto> CreateTransactionAsync(CreateSupplierTransactionDto createDto);
    Task<SupplierTransactionDto> UpdateTransactionAsync(UpdateSupplierTransactionDto updateDto);
    Task<bool> DeleteTransactionAsync(int id);
}
