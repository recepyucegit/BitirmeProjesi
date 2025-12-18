namespace TeknoRoma.Domain.Entities;

public class UserRole : BaseEntity
{
    public int UserId { get; set; }
    public int RoleId { get; set; }

    // Navigation Properties
    public virtual User? User { get; set; }
    public virtual Role? Role { get; set; }
}
