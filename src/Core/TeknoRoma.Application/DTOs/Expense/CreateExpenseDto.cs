namespace TeknoRoma.Application.DTOs.Expense;

public class CreateExpenseDto
{
    public string ExpenseType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "TL";
    public decimal ExchangeRate { get; set; } = 1;
    public DateTime ExpenseDate { get; set; } = DateTime.Now;
    public int? EmployeeId { get; set; }
    public int? StoreId { get; set; }
    public string? InvoiceNumber { get; set; }
    public string? Vendor { get; set; }
    public string Category { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = "BankTransfer";
    public string? Notes { get; set; }
}
