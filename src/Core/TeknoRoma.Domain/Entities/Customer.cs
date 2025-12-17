using System.ComponentModel.DataAnnotations;

namespace TeknoRoma.Domain.Entities;

public class Customer : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string IdentityNumber { get; set; } = string.Empty; // TC Kimlik No
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string CustomerType { get; set; } = "Individual"; // Individual, Corporate
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    // public virtual ICollection<Sale>? Sales { get; set; }
}
