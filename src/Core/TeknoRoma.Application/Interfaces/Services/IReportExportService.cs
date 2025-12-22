namespace TeknoRoma.Application.Interfaces.Services;

public interface IReportExportService
{
    /// <summary>
    /// Export sales report to Excel file
    /// </summary>
    Task<byte[]> ExportSalesReportAsync(DateTime? startDate, DateTime? endDate, int? categoryId, int? storeId);

    /// <summary>
    /// Export stock report to Excel file
    /// </summary>
    Task<byte[]> ExportStockReportAsync(int? categoryId, bool? lowStockOnly);

    /// <summary>
    /// Export expense report to Excel file
    /// </summary>
    Task<byte[]> ExportExpenseReportAsync(DateTime? startDate, DateTime? endDate, int? storeId, int? departmentId, string? category, string? status);
}
