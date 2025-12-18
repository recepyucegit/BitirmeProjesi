namespace TeknoRoma.Application.DTOs.Store;

public class StoreDto
{
    public int Id { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public string StoreCode { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? District { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int? ManagerId { get; set; }
    public string? ManagerName { get; set; }
    public bool IsActive { get; set; }
    public DateTime OpeningDate { get; set; }
    public decimal? MonthlyTarget { get; set; }
    public int? Capacity { get; set; }
    public int EmployeeCount { get; set; }
    public DateTime CreatedDate { get; set; }
}
