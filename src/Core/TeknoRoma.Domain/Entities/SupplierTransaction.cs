namespace TeknoRoma.Domain.Entities;

public class SupplierTransaction : BaseEntity
{
    public int SupplierId { get; set; }
    public int? ProductId { get; set; }
    public string TransactionType { get; set; } = string.Empty; // Purchase, Return, Payment, etc.
    public decimal Amount { get; set; }
    public int? Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public string? Description { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    public string? InvoiceNumber { get; set; }
    public string? ReferenceNumber { get; set; }
    public string Status { get; set; } = "Completed"; // Pending, Completed, Cancelled

    // Navigation Properties
    public virtual Supplier? Supplier { get; set; }
    public virtual Product? Product { get; set; }
}
