namespace TeknoRoma.Domain.Entities;

public class SaleDetail : BaseEntity
{
    public int SaleId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountRate { get; set; } = 0;
    public decimal DiscountAmount { get; set; } = 0;
    public decimal TotalPrice { get; set; }
    public decimal NetPrice { get; set; }

    // Navigation Properties
    public virtual Sale? Sale { get; set; }
    public virtual Product? Product { get; set; }
}
