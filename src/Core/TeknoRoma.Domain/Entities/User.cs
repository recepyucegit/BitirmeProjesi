namespace TeknoRoma.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginDate { get; set; }
    public int? EmployeeId { get; set; } // Optional - Her user employee olmayabilir
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    // Navigation Properties
    public virtual Employee? Employee { get; set; }
    public virtual ICollection<UserRole>? UserRoles { get; set; }
}
