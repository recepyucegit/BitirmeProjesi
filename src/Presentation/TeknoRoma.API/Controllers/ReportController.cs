using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeknoRoma.Application.Common;
using TeknoRoma.Application.DTOs.Report;
using TeknoRoma.Application.Interfaces.Services;

namespace TeknoRoma.API.Controllers;

/// <summary>
/// Controller for reporting and analytics operations
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// Get paginated sales report with filtering options
    /// </summary>
    [HttpGet("sales")]
    public async Task<ActionResult<PagedResult<SalesReportDto>>> GetSalesReport(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int? categoryId,
        [FromQuery] int? storeId,
        [FromQuery] PaginationParams pagination)
    {
        var report = await _reportService.GetSalesReportAsync(
            startDate, endDate, categoryId, storeId, pagination);
        return Ok(report);
    }

    /// <summary>
    /// Get sales summary with aggregated statistics
    /// </summary>
    [HttpGet("sales/summary")]
    public async Task<ActionResult<SalesSummaryDto>> GetSalesSummary(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var summary = await _reportService.GetSalesSummaryAsync(startDate, endDate);
        return Ok(summary);
    }

    /// <summary>
    /// Get top selling products by revenue
    /// </summary>
    [HttpGet("sales/top-products")]
    public async Task<ActionResult<List<TopProductDto>>> GetTopSellingProducts(
        [FromQuery] int count = 10,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var products = await _reportService.GetTopSellingProductsAsync(count, startDate, endDate);
        return Ok(products);
    }

    /// <summary>
    /// Get top customers by total revenue
    /// </summary>
    [HttpGet("sales/top-customers")]
    public async Task<ActionResult<List<TopCustomerDto>>> GetTopCustomers(
        [FromQuery] int count = 10,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var customers = await _reportService.GetTopCustomersAsync(count, startDate, endDate);
        return Ok(customers);
    }

    /// <summary>
    /// Get top performing employees by sales count
    /// </summary>
    [HttpGet("sales/top-employees")]
    public async Task<ActionResult<List<TopEmployeeDto>>> GetTopEmployees(
        [FromQuery] int count = 10,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var employees = await _reportService.GetTopEmployeesAsync(count, startDate, endDate);
        return Ok(employees);
    }

    /// <summary>
    /// Get paginated stock/inventory report
    /// </summary>
    [HttpGet("stock")]
    public async Task<ActionResult<PagedResult<StockReportDto>>> GetStockReport(
        [FromQuery] int? categoryId,
        [FromQuery] bool? lowStockOnly,
        [FromQuery] PaginationParams pagination)
    {
        var report = await _reportService.GetStockReportAsync(categoryId, lowStockOnly, pagination);
        return Ok(report);
    }

    /// <summary>
    /// Get low stock alerts for products below critical level
    /// </summary>
    [HttpGet("stock/low-stock-alerts")]
    public async Task<ActionResult<List<LowStockProductDto>>> GetLowStockAlerts()
    {
        var alerts = await _reportService.GetLowStockProductsAsync();
        return Ok(alerts);
    }

    /// <summary>
    /// Get stock summary with category breakdown
    /// </summary>
    [HttpGet("stock/summary")]
    public async Task<ActionResult<StockSummaryDto>> GetStockSummary()
    {
        var summary = await _reportService.GetStockSummaryAsync();
        return Ok(summary);
    }

    /// <summary>
    /// Get paginated expense report with filtering options
    /// </summary>
    [HttpGet("expenses")]
    public async Task<ActionResult<PagedResult<ExpenseReportDto>>> GetExpenseReport(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int? storeId,
        [FromQuery] int? departmentId,
        [FromQuery] string? category,
        [FromQuery] string? status,
        [FromQuery] PaginationParams pagination)
    {
        var report = await _reportService.GetExpenseReportAsync(
            startDate, endDate, storeId, departmentId, category, status, pagination);
        return Ok(report);
    }

    /// <summary>
    /// Get expense summary with aggregated statistics
    /// </summary>
    [HttpGet("expenses/summary")]
    public async Task<ActionResult<ExpenseSummaryDto>> GetExpenseSummary(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var summary = await _reportService.GetExpenseSummaryAsync(startDate, endDate);
        return Ok(summary);
    }

    /// <summary>
    /// Get comprehensive dashboard statistics
    /// </summary>
    [HttpGet("dashboard")]
    public async Task<ActionResult<DashboardStatsDto>> GetDashboardStats()
    {
        var stats = await _reportService.GetDashboardStatsAsync();
        return Ok(stats);
    }

}
