namespace TeknoRoma.Application.DTOs.User;

public class UpdateUserDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public int? EmployeeId { get; set; }
    public List<int> RoleIds { get; set; } = new();
}
