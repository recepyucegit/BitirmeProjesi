using TeknoRoma.Application.Common;
using TeknoRoma.Application.DTOs.Report;

namespace TeknoRoma.Application.Interfaces.Repositories;

public interface IReportRepository
{
    // Sales Reports
    Task<PagedResult<SalesReportDto>> GetSalesReportAsync(
        DateTime? startDate,
        DateTime? endDate,
        int? categoryId,
        int? storeId,
        PaginationParams paginationParams);

    Task<SalesSummaryDto> GetSalesSummaryAsync(DateTime? startDate, DateTime? endDate);

    Task<List<TopProductDto>> GetTopSellingProductsAsync(int count, DateTime? startDate, DateTime? endDate);

    Task<List<TopCustomerDto>> GetTopCustomersAsync(int count, DateTime? startDate, DateTime? endDate);

    Task<List<TopEmployeeDto>> GetTopEmployeesAsync(int count, DateTime? startDate, DateTime? endDate);

    // Stock Reports
    Task<PagedResult<StockReportDto>> GetStockReportAsync(
        int? categoryId,
        bool? lowStockOnly,
        PaginationParams paginationParams);

    Task<List<LowStockProductDto>> GetLowStockProductsAsync();

    Task<StockSummaryDto> GetStockSummaryAsync();

    // Expense Reports
    Task<PagedResult<ExpenseReportDto>> GetExpenseReportAsync(
        DateTime? startDate,
        DateTime? endDate,
        int? storeId,
        int? departmentId,
        string? category,
        string? status,
        PaginationParams paginationParams);

    Task<ExpenseSummaryDto> GetExpenseSummaryAsync(DateTime? startDate, DateTime? endDate);

    // Dashboard
    Task<DashboardStatsDto> GetDashboardStatsAsync();
}
