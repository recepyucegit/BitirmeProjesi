namespace TeknoRoma.Application.DTOs.Report;

public class DashboardStatsDto
{
    // Sales Stats
    public decimal TodaySales { get; set; }
    public decimal WeeklySales { get; set; }
    public decimal MonthlySales { get; set; }
    public decimal YearlySales { get; set; }
    public int TodaySalesCount { get; set; }
    public int WeeklySalesCount { get; set; }
    public int MonthlySalesCount { get; set; }

    // Product Stats
    public int TotalProducts { get; set; }
    public int ActiveProducts { get; set; }
    public int LowStockProducts { get; set; }
    public int OutOfStockProducts { get; set; }
    public decimal TotalStockValue { get; set; }

    // Customer Stats
    public int TotalCustomers { get; set; }
    public int NewCustomersThisMonth { get; set; }
    public int ActiveCustomers { get; set; }

    // Employee Stats
    public int TotalEmployees { get; set; }
    public int ActiveEmployees { get; set; }

    // Store Stats
    public int TotalStores { get; set; }
    public int ActiveStores { get; set; }

    // Expense Stats
    public decimal MonthlyExpenses { get; set; }
    public int PendingExpenses { get; set; }

    // Top Performers
    public List<TopProductDto> TopSellingProducts { get; set; } = new();
    public List<TopCustomerDto> TopCustomers { get; set; } = new();
    public List<TopEmployeeDto> TopEmployees { get; set; } = new();

    // Recent Activities
    public List<RecentSaleDto> RecentSales { get; set; } = new();
    public List<LowStockAlertDto> LowStockAlerts { get; set; } = new();
}

public class TopProductDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int TotalQuantitySold { get; set; }
    public decimal TotalRevenue { get; set; }
}

public class TopCustomerDto
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int TotalPurchases { get; set; }
    public decimal TotalSpent { get; set; }
    public DateTime? LastPurchaseDate { get; set; }
}

public class TopEmployeeDto
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string? StoreName { get; set; }
    public int TotalSales { get; set; }
    public decimal TotalRevenue { get; set; }
}

public class RecentSaleDto
{
    public int SaleId { get; set; }
    public DateTime SaleDate { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string? EmployeeName { get; set; }
}

public class LowStockAlertDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int CurrentStock { get; set; }
    public int MinimumStock { get; set; }
    public string? SupplierName { get; set; }
}
