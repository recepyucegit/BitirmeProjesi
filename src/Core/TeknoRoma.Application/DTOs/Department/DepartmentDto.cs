namespace TeknoRoma.Application.DTOs.Department;

public class DepartmentDto
{
    public int Id { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string DepartmentCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? ManagerId { get; set; }
    public string? ManagerName { get; set; }
    public decimal? Budget { get; set; }
    public int? EmployeeCount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}
