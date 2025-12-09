namespace TeknoRoma.Domain.Entities;

public class Supplier : BaseEntity
{
    public string CompanyName { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string? ContactTitle { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? TaxNumber { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public virtual ICollection<Product>? Products { get; set; }
    // public virtual ICollection<SupplierTransaction>? SupplierTransactions { get; set; }
}
