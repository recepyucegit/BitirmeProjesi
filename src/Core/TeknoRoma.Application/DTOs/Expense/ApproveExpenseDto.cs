namespace TeknoRoma.Application.DTOs.Expense;

public class ApproveExpenseDto
{
    public int ApprovedBy { get; set; }
    public bool IsApproved { get; set; } // true = Approved, false = Rejected
    public string? Notes { get; set; }
}
