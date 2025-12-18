namespace TeknoRoma.Application.DTOs.Department;

public class CreateDepartmentDto
{
    public string DepartmentName { get; set; } = string.Empty;
    public string DepartmentCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? ManagerId { get; set; }
    public decimal? Budget { get; set; }
    public int? EmployeeCount { get; set; }
    public bool IsActive { get; set; } = true;
}
