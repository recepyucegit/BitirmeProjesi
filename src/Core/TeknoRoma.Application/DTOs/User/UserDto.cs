namespace TeknoRoma.Application.DTOs.User;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public int? EmployeeId { get; set; }
    public string? EmployeeFullName { get; set; }
    public List<string> Roles { get; set; } = new();
    public DateTime CreatedDate { get; set; }
}
