using TeknoRoma.Application.DTOs.Sale;

namespace TeknoRoma.Application.Interfaces.Services;

public interface ISaleService
{
    Task<IEnumerable<SaleDto>> GetAllSalesAsync();
    Task<IEnumerable<SaleDto>> GetSalesWithDetailsAsync();
    Task<SaleDto?> GetSaleByIdAsync(int id);
    Task<SaleDto?> GetSaleWithDetailsAsync(int id);
    Task<IEnumerable<SaleDto>> GetSalesByCustomerIdAsync(int customerId);
    Task<IEnumerable<SaleDto>> GetSalesByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<SaleDto>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<SaleDto>> GetSalesByStatusAsync(string status);
    Task<SaleDto> CreateSaleAsync(CreateSaleDto createSaleDto);
    Task<SaleDto> UpdateSaleAsync(UpdateSaleDto updateSaleDto);
    Task<bool> DeleteSaleAsync(int id);
    Task<decimal> GetTotalSalesAmountByEmployeeAsync(int employeeId, DateTime startDate, DateTime endDate);
    Task<decimal> GetTotalCommissionByEmployeeAsync(int employeeId, DateTime startDate, DateTime endDate);
}
