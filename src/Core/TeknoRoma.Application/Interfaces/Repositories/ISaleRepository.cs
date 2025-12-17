using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Application.Interfaces.Repositories;

public interface ISaleRepository : IRepository<Sale>
{
    Task<Sale?> GetSaleWithDetailsAsync(int id);
    Task<IEnumerable<Sale>> GetSalesByCustomerIdAsync(int customerId);
    Task<IEnumerable<Sale>> GetSalesByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Sale>> GetSalesByStatusAsync(string status);
    Task<decimal> GetTotalSalesAmountByEmployeeAsync(int employeeId, DateTime startDate, DateTime endDate);
    Task<decimal> GetTotalCommissionByEmployeeAsync(int employeeId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<Sale>> GetSalesWithDetailsAsync();
}
