using TeknoRoma.Application.Common;
using TeknoRoma.Application.DTOs.Report;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Application.Interfaces.Services;

namespace TeknoRoma.Infrastructure.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;

    public ReportService(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    #region Sales Reports

    public async Task<PagedResult<SalesReportDto>> GetSalesReportAsync(
        DateTime? startDate,
        DateTime? endDate,
        int? categoryId,
        int? storeId,
        PaginationParams paginationParams)
    {
        return await _reportRepository.GetSalesReportAsync(
            startDate,
            endDate,
            categoryId,
            storeId,
            paginationParams);
    }

    public async Task<SalesSummaryDto> GetSalesSummaryAsync(DateTime? startDate, DateTime? endDate)
    {
        // Default to current month if no dates provided
        if (!startDate.HasValue && !endDate.HasValue)
        {
            var now = DateTime.Now;
            startDate = new DateTime(now.Year, now.Month, 1);
            endDate = now;
        }

        return await _reportRepository.GetSalesSummaryAsync(startDate, endDate);
    }

    public async Task<List<TopProductDto>> GetTopSellingProductsAsync(
        int count = 10,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        if (count <= 0 || count > 100)
            count = 10;

        return await _reportRepository.GetTopSellingProductsAsync(count, startDate, endDate);
    }

    #endregion

    #region Stock Reports

    public async Task<PagedResult<StockReportDto>> GetStockReportAsync(
        int? categoryId,
        bool? lowStockOnly,
        PaginationParams paginationParams)
    {
        return await _reportRepository.GetStockReportAsync(
            categoryId,
            lowStockOnly,
            paginationParams);
    }

    public async Task<List<LowStockProductDto>> GetLowStockProductsAsync()
    {
        return await _reportRepository.GetLowStockProductsAsync();
    }

    public async Task<StockSummaryDto> GetStockSummaryAsync()
    {
        return await _reportRepository.GetStockSummaryAsync();
    }

    #endregion

    #region Expense Reports

    public async Task<PagedResult<ExpenseReportDto>> GetExpenseReportAsync(
        DateTime? startDate,
        DateTime? endDate,
        int? storeId,
        int? departmentId,
        string? category,
        string? status,
        PaginationParams paginationParams)
    {
        return await _reportRepository.GetExpenseReportAsync(
            startDate,
            endDate,
            storeId,
            departmentId,
            category,
            status,
            paginationParams);
    }

    public async Task<ExpenseSummaryDto> GetExpenseSummaryAsync(DateTime? startDate, DateTime? endDate)
    {
        // Default to current month if no dates provided
        if (!startDate.HasValue && !endDate.HasValue)
        {
            var now = DateTime.Now;
            startDate = new DateTime(now.Year, now.Month, 1);
            endDate = now;
        }

        return await _reportRepository.GetExpenseSummaryAsync(startDate, endDate);
    }

    #endregion

    #region Dashboard

    public async Task<DashboardStatsDto> GetDashboardStatsAsync()
    {
        return await _reportRepository.GetDashboardStatsAsync();
    }

    #endregion

    #region Customer Reports

    public async Task<List<TopCustomerDto>> GetTopCustomersAsync(
        int count = 10,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        if (count <= 0 || count > 100)
            count = 10;

        return await _reportRepository.GetTopCustomersAsync(count, startDate, endDate);
    }

    #endregion

    #region Employee Performance

    public async Task<List<TopEmployeeDto>> GetTopEmployeesAsync(
        int count = 10,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        if (count <= 0 || count > 100)
            count = 10;

        return await _reportRepository.GetTopEmployeesAsync(count, startDate, endDate);
    }

    #endregion
}
