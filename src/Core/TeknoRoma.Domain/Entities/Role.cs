namespace TeknoRoma.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty; // Admin, BranchManager, Cashier, Warehouse, Accounting, TechnicalService
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public virtual ICollection<UserRole>? UserRoles { get; set; }
}
