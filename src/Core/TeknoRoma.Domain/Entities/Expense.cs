namespace TeknoRoma.Domain.Entities;

public class Expense : BaseEntity
{
    public string ExpenseType { get; set; } = string.Empty; // EmployeePayment, Infrastructure, Invoice, Other
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "TL"; // TL, USD, EUR
    public decimal ExchangeRate { get; set; } = 1; // Döviz kuru
    public decimal AmountInTL { get; set; } // TL karşılığı
    public DateTime ExpenseDate { get; set; } = DateTime.Now;
    public int? EmployeeId { get; set; } // Gideri onaylayan veya ilişkili çalışan
    public int? StoreId { get; set; } // Şubeye özel giderler
    public string? InvoiceNumber { get; set; }
    public string? Vendor { get; set; } // Tedarikçi/Satıcı
    public string Category { get; set; } = string.Empty; // Salary, Rent, Utilities, Maintenance, Marketing, etc.
    public string PaymentMethod { get; set; } = "BankTransfer"; // Cash, BankTransfer, CreditCard
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Paid
    public int? ApprovedBy { get; set; } // Onaylayan yönetici
    public DateTime? ApprovalDate { get; set; }
    public string? Notes { get; set; }

    // Navigation Properties
    public virtual Employee? Employee { get; set; }
    public virtual Store? Store { get; set; }
    public virtual Employee? Approver { get; set; }
}
