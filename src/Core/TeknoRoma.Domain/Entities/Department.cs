namespace TeknoRoma.Domain.Entities;

public class Department : BaseEntity
{
    public string DepartmentName { get; set; } = string.Empty;
    public string DepartmentCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? ManagerId { get; set; } // Department manager (Employee)
    public decimal? Budget { get; set; } // Monthly budget
    public int? EmployeeCount { get; set; } // Number of employees
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public virtual Employee? Manager { get; set; }
    public virtual ICollection<Employee>? Employees { get; set; }
}
