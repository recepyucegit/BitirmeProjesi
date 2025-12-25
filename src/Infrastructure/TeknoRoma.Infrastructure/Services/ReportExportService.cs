using OfficeOpenXml;
using OfficeOpenXml.Style;
using TeknoRoma.Application.Common;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Application.Interfaces.Services;

namespace TeknoRoma.Infrastructure.Services;

public class ReportExportService : IReportExportService
{
    private readonly IReportRepository _reportRepository;

    public ReportExportService(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
        // Set EPPlus license context for non-commercial use (EPPlus 7.x)
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public async Task<byte[]> ExportSalesReportAsync(DateTime? startDate, DateTime? endDate, int? categoryId, int? storeId)
    {
        // Get sales data with default pagination (all records)
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 100000 };
        var salesData = await _reportRepository.GetSalesReportAsync(startDate, endDate, categoryId, storeId, pagination);

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Sales Report");

        // Header
        worksheet.Cells[1, 1].Value = "Sale Date";
        worksheet.Cells[1, 2].Value = "Invoice No";
        worksheet.Cells[1, 3].Value = "Customer";
        worksheet.Cells[1, 4].Value = "Employee";
        worksheet.Cells[1, 5].Value = "Store";
        worksheet.Cells[1, 6].Value = "Subtotal";
        worksheet.Cells[1, 7].Value = "Tax";
        worksheet.Cells[1, 8].Value = "Discount";
        worksheet.Cells[1, 9].Value = "Total Amount";
        worksheet.Cells[1, 10].Value = "Payment Method";

        // Style header
        using (var range = worksheet.Cells[1, 1, 1, 10])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        // Data rows
        int row = 2;
        foreach (var sale in salesData.Items)
        {
            worksheet.Cells[row, 1].Value = sale.SaleDate.ToString("dd/MM/yyyy HH:mm");
            worksheet.Cells[row, 2].Value = sale.SaleId;
            worksheet.Cells[row, 3].Value = sale.CustomerName;
            worksheet.Cells[row, 4].Value = sale.EmployeeName;
            worksheet.Cells[row, 5].Value = sale.StoreName;
            worksheet.Cells[row, 6].Value = sale.TotalItems;
            worksheet.Cells[row, 7].Value = "";
            worksheet.Cells[row, 8].Value = "";
            worksheet.Cells[row, 9].Value = sale.TotalAmount;
            worksheet.Cells[row, 10].Value = sale.PaymentMethod;

            // Format currency columns
            worksheet.Cells[row, 9].Style.Numberformat.Format = "#,##0.00";

            row++;
        }

        // Auto-fit columns
        worksheet.Cells.AutoFitColumns();

        // Add summary row
        worksheet.Cells[row + 1, 8].Value = "Total:";
        worksheet.Cells[row + 1, 8].Style.Font.Bold = true;
        worksheet.Cells[row + 1, 9].Formula = $"SUM(I2:I{row - 1})";
        worksheet.Cells[row + 1, 9].Style.Font.Bold = true;
        worksheet.Cells[row + 1, 9].Style.Numberformat.Format = "#,##0.00";

        return package.GetAsByteArray();
    }

    public async Task<byte[]> ExportStockReportAsync(int? categoryId, bool? lowStockOnly)
    {
        // Get stock data with default pagination (all records)
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 100000 };
        var stockData = await _reportRepository.GetStockReportAsync(categoryId, lowStockOnly, pagination);

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Stock Report");

        // Header
        worksheet.Cells[1, 1].Value = "Product ID";
        worksheet.Cells[1, 2].Value = "Product Name";
        worksheet.Cells[1, 3].Value = "Category";
        worksheet.Cells[1, 4].Value = "Stock Quantity";
        worksheet.Cells[1, 5].Value = "Minimum Level";
        worksheet.Cells[1, 6].Value = "Unit Price";
        worksheet.Cells[1, 7].Value = "Total Value";
        worksheet.Cells[1, 8].Value = "Status";

        // Style header
        using (var range = worksheet.Cells[1, 1, 1, 8])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        // Data rows
        int row = 2;
        foreach (var stock in stockData.Items)
        {
            worksheet.Cells[row, 1].Value = stock.ProductId;
            worksheet.Cells[row, 2].Value = stock.ProductName;
            worksheet.Cells[row, 3].Value = stock.CategoryName;
            worksheet.Cells[row, 4].Value = stock.StockQuantity;
            worksheet.Cells[row, 5].Value = stock.MinimumStockLevel;
            worksheet.Cells[row, 6].Value = stock.UnitPrice;
            worksheet.Cells[row, 7].Value = stock.TotalValue;
            worksheet.Cells[row, 8].Value = stock.IsLowStock ? "LOW STOCK" : "OK";

            // Format currency columns
            worksheet.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
            worksheet.Cells[row, 7].Style.Numberformat.Format = "#,##0.00";

            // Highlight low stock rows
            if (stock.IsLowStock)
            {
                using (var range = worksheet.Cells[row, 1, row, 8])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);
                }
            }

            row++;
        }

        // Auto-fit columns
        worksheet.Cells.AutoFitColumns();

        // Add summary
        worksheet.Cells[row + 1, 6].Value = "Total Value:";
        worksheet.Cells[row + 1, 6].Style.Font.Bold = true;
        worksheet.Cells[row + 1, 7].Formula = $"SUM(G2:G{row - 1})";
        worksheet.Cells[row + 1, 7].Style.Font.Bold = true;
        worksheet.Cells[row + 1, 7].Style.Numberformat.Format = "#,##0.00";

        return package.GetAsByteArray();
    }

    public async Task<byte[]> ExportExpenseReportAsync(DateTime? startDate, DateTime? endDate, int? storeId, int? departmentId, string? category, string? status)
    {
        // Get expense data with default pagination (all records)
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 100000 };
        var expenseData = await _reportRepository.GetExpenseReportAsync(startDate, endDate, storeId, departmentId, category, status, pagination);

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Expense Report");

        // Header
        worksheet.Cells[1, 1].Value = "Expense Date";
        worksheet.Cells[1, 2].Value = "Category";
        worksheet.Cells[1, 3].Value = "Description";
        worksheet.Cells[1, 4].Value = "Amount";
        worksheet.Cells[1, 5].Value = "Store";
        worksheet.Cells[1, 6].Value = "Department";
        worksheet.Cells[1, 7].Value = "Status";
        worksheet.Cells[1, 8].Value = "Payment Method";
        worksheet.Cells[1, 9].Value = "Invoice No";

        // Style header
        using (var range = worksheet.Cells[1, 1, 1, 9])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        // Data rows
        int row = 2;
        foreach (var expense in expenseData.Items)
        {
            worksheet.Cells[row, 1].Value = expense.ExpenseDate.ToString("dd/MM/yyyy");
            worksheet.Cells[row, 2].Value = expense.Category;
            worksheet.Cells[row, 3].Value = expense.Description;
            worksheet.Cells[row, 4].Value = expense.Amount;
            worksheet.Cells[row, 5].Value = expense.StoreName;
            worksheet.Cells[row, 6].Value = expense.DepartmentName;
            worksheet.Cells[row, 7].Value = expense.Status;
            worksheet.Cells[row, 8].Value = expense.PaymentMethod;
            worksheet.Cells[row, 9].Value = expense.InvoiceNumber;

            // Format currency column
            worksheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";

            // Color code by status
            if (expense.Status == "Pending")
            {
                using (var statusCell = worksheet.Cells[row, 7])
                {
                    statusCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    statusCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);
                }
            }
            else if (expense.Status == "Approved")
            {
                using (var statusCell = worksheet.Cells[row, 7])
                {
                    statusCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    statusCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                }
            }

            row++;
        }

        // Auto-fit columns
        worksheet.Cells.AutoFitColumns();

        // Add summary
        worksheet.Cells[row + 1, 3].Value = "Total Expenses:";
        worksheet.Cells[row + 1, 3].Style.Font.Bold = true;
        worksheet.Cells[row + 1, 4].Formula = $"SUM(D2:D{row - 1})";
        worksheet.Cells[row + 1, 4].Style.Font.Bold = true;
        worksheet.Cells[row + 1, 4].Style.Numberformat.Format = "#,##0.00";

        return package.GetAsByteArray();
    }
}
