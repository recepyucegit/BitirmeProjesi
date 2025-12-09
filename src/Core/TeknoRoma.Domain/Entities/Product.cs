namespace TeknoRoma.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public decimal Price { get; set; }
    public decimal CostPrice { get; set; }
    public int StockQuantity { get; set; }
    public int CriticalStockLevel { get; set; } = 10;
    public bool IsActive { get; set; } = true;
    public string? ImageUrl { get; set; }

    // Foreign Keys
    public int CategoryId { get; set; }
    public int? SupplierId { get; set; }

    // Navigation Properties
    public virtual Category? Category { get; set; }
    public virtual Supplier? Supplier { get; set; }
    // public virtual ICollection<SaleDetail>? SaleDetails { get; set; }
    // public virtual ICollection<SupplierTransaction>? SupplierTransactions { get; set; }
}
