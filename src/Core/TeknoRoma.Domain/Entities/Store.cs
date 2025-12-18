namespace TeknoRoma.Domain.Entities;

public class Store : BaseEntity
{
    public string StoreName { get; set; } = string.Empty;
    public string StoreCode { get; set; } = string.Empty; // Unique store identifier
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? District { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int? ManagerId { get; set; } // Store Manager (Employee)
    public bool IsActive { get; set; } = true;
    public DateTime OpeningDate { get; set; }
    public decimal? MonthlyTarget { get; set; } // Monthly sales target
    public int? Capacity { get; set; } // Store capacity (square meters)

    // Navigation Properties
    public virtual Employee? Manager { get; set; }
    public virtual ICollection<Employee>? Employees { get; set; }
    public virtual ICollection<Sale>? Sales { get; set; }
}
