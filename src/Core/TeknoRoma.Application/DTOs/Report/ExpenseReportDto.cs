namespace TeknoRoma.Application.DTOs.Report;

public class ExpenseReportDto
{
    public int ExpenseId { get; set; }
    public DateTime ExpenseDate { get; set; }
    public string ExpenseType { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "TL";
    public decimal AmountInTL { get; set; }
    public string? EmployeeName { get; set; }
    public string? StoreName { get; set; }
    public string? DepartmentName { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ApproverName { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? Vendor { get; set; }
    public string? InvoiceNumber { get; set; }
}

public class ExpenseSummaryDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalExpenseCount { get; set; }
    public decimal TotalExpenseAmount { get; set; }
    public decimal AverageExpenseAmount { get; set; }
    public int PendingExpenseCount { get; set; }
    public int ApprovedExpenseCount { get; set; }
    public int RejectedExpenseCount { get; set; }
    public Dictionary<string, decimal> ExpensesByCategory { get; set; } = new();
    public Dictionary<string, decimal> ExpensesByStore { get; set; } = new();
    public Dictionary<string, decimal> ExpensesByDepartment { get; set; } = new();
    public Dictionary<string, decimal> ExpensesByCurrency { get; set; } = new();
}
