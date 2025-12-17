namespace TeknoRoma.Domain.Entities;

public class SupplierTransactionDetail : BaseEntity
{
    public int SupplierTransactionId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }

    // Navigation Properties
    public virtual SupplierTransaction? SupplierTransaction { get; set; }
    public virtual Product? Product { get; set; }
}
