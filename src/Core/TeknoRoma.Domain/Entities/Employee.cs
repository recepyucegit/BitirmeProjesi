namespace TeknoRoma.Domain.Entities;

public class Employee : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string IdentityNumber { get; set; } = string.Empty; // TC Kimlik No
    public DateTime HireDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public decimal Salary { get; set; }
    public decimal SalesQuota { get; set; } = 10000; // Satış kotası (10.000 TL)
    public decimal CommissionRate { get; set; } = 0.10m; // Prim oranı (%10)
    public string Role { get; set; } = string.Empty; // Şube Müdürü, Kasa Satış, Mobil Satış, Depo, Muhasebe, Teknik Servis
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int? StoreId { get; set; } // Store/Branch assignment
    public int? DepartmentId { get; set; } // Department assignment

    // Navigation Properties
    public virtual Store? Store { get; set; }
    public virtual Department? Department { get; set; }
    // public virtual ICollection<Sale>? Sales { get; set; }
    // public virtual ICollection<Expense>? Expenses { get; set; }
}
