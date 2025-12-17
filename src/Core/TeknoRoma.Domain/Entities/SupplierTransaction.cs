namespace TeknoRoma.Domain.Entities;

public class SupplierTransaction : BaseEntity
{
    public int SupplierId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public DateTime? DeliveryDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Ordered"; // Ordered, Delivered, Cancelled
    public string? Notes { get; set; }
    public string OrderNumber { get; set; } = string.Empty;

    // Navigation Properties
    public virtual Supplier? Supplier { get; set; }
    public virtual Employee? Employee { get; set; }
    public virtual ICollection<SupplierTransactionDetail>? Details { get; set; }
}
