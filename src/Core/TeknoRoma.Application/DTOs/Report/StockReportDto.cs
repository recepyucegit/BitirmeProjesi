namespace TeknoRoma.Application.DTOs.Report;

public class StockReportDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public int StockQuantity { get; set; }
    public int MinimumStockLevel { get; set; }
    public bool IsLowStock => StockQuantity <= MinimumStockLevel;
    public decimal UnitPrice { get; set; }
    public decimal TotalValue => StockQuantity * UnitPrice;
    public string? SupplierName { get; set; }
    public DateTime? LastRestockDate { get; set; }
}

public class LowStockProductDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int CurrentStock { get; set; }
    public int MinimumStock { get; set; }
    public int StockDeficit => MinimumStock - CurrentStock;
    public string? SupplierName { get; set; }
    public string? SupplierPhone { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal ReorderCost => StockDeficit * UnitPrice;
}

public class StockSummaryDto
{
    public int TotalProducts { get; set; }
    public int LowStockProducts { get; set; }
    public int OutOfStockProducts { get; set; }
    public decimal TotalStockValue { get; set; }
    public Dictionary<string, int> StockByCategory { get; set; } = new();
    public Dictionary<string, decimal> ValueByCategory { get; set; } = new();
}
