namespace TeknoRoma.Domain.Entities;

public class Sale : BaseEntity
{
    public DateTime SaleDate { get; set; } = DateTime.Now;
    public int CustomerId { get; set; }
    public int EmployeeId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; } = 0;
    public decimal NetAmount { get; set; }
    public decimal CommissionAmount { get; set; }
    public string PaymentMethod { get; set; } = "Cash"; // Cash, CreditCard, BankTransfer
    public string? Notes { get; set; }
    public string Status { get; set; } = "Completed"; // Completed, Cancelled, Refunded
    public string InvoiceNumber { get; set; } = string.Empty;

    // Navigation Properties
    public virtual Customer? Customer { get; set; }
    public virtual Employee? Employee { get; set; }
    public virtual ICollection<SaleDetail>? SaleDetails { get; set; }
}
