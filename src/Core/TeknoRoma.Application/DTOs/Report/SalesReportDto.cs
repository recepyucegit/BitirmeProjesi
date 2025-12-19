namespace TeknoRoma.Application.DTOs.Report;

public class SalesReportDto
{
    public int SaleId { get; set; }
    public DateTime SaleDate { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerTC { get; set; }
    public string? EmployeeName { get; set; }
    public string? StoreName { get; set; }
    public int TotalItems { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public List<SalesReportItemDto> Items { get; set; } = new();
}

public class SalesReportItemDto
{
    public string ProductName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalPrice { get; set; }
}

public class SalesSummaryDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalSalesCount { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal AverageSaleAmount { get; set; }
    public int TotalProductsSold { get; set; }
    public int UniqueCustomers { get; set; }
    public Dictionary<string, decimal> SalesByCategory { get; set; } = new();
    public Dictionary<string, decimal> SalesByStore { get; set; } = new();
    public Dictionary<string, int> SalesByPaymentMethod { get; set; } = new();
}
