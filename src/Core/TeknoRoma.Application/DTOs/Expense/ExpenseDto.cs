namespace TeknoRoma.Application.DTOs.Expense;

public class ExpenseDto
{
    public int Id { get; set; }
    public string ExpenseType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "TL";
    public decimal ExchangeRate { get; set; }
    public decimal AmountInTL { get; set; }
    public DateTime ExpenseDate { get; set; }
    public int? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public int? StoreId { get; set; }
    public string? StoreName { get; set; }
    public string? InvoiceNumber { get; set; }
    public string? Vendor { get; set; }
    public string Category { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int? ApprovedBy { get; set; }
    public string? ApproverName { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; }
}
