using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeknoRoma.Application.DTOs.Expense;
using TeknoRoma.Application.Interfaces.Services;

namespace TeknoRoma.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ExpenseController : ControllerBase
{
    private readonly IExpenseService _expenseService;

    public ExpenseController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Accounting,StoreManager")]
    public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetAllExpenses()
    {
        var expenses = await _expenseService.GetAllAsync();
        return Ok(expenses);
    }

    [HttpGet("pending")]
    [Authorize(Roles = "Admin,Accounting,StoreManager")]
    public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetPendingExpenses()
    {
        var expenses = await _expenseService.GetPendingExpensesAsync();
        return Ok(expenses);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ExpenseDto>> GetExpense(int id)
    {
        var expense = await _expenseService.GetByIdAsync(id);
        if (expense == null) return NotFound();
        return Ok(expense);
    }

    [HttpGet("store/{storeId}")]
    public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesByStore(int storeId)
    {
        var expenses = await _expenseService.GetByStoreIdAsync(storeId);
        return Ok(expenses);
    }

    [HttpGet("status/{status}")]
    [Authorize(Roles = "Admin,Accounting,StoreManager")]
    public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesByStatus(string status)
    {
        var expenses = await _expenseService.GetByStatusAsync(status);
        return Ok(expenses);
    }

    [HttpGet("date-range")]
    [Authorize(Roles = "Admin,Accounting,StoreManager")]
    public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesByDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var expenses = await _expenseService.GetByDateRangeAsync(startDate, endDate);
        return Ok(expenses);
    }

    [HttpGet("store/{storeId}/total")]
    [Authorize(Roles = "Admin,Accounting,StoreManager")]
    public async Task<ActionResult<decimal>> GetTotalExpensesByStore(
        int storeId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var total = await _expenseService.GetTotalExpensesByStoreAsync(storeId, startDate, endDate);
        return Ok(total);
    }

    [HttpGet("category/{category}/total")]
    [Authorize(Roles = "Admin,Accounting")]
    public async Task<ActionResult<decimal>> GetTotalExpensesByCategory(
        string category,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var total = await _expenseService.GetTotalExpensesByCategoryAsync(category, startDate, endDate);
        return Ok(total);
    }

    [HttpPost]
    public async Task<ActionResult<ExpenseDto>> CreateExpense(CreateExpenseDto dto)
    {
        var expense = await _expenseService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expense);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ExpenseDto>> UpdateExpense(int id, UpdateExpenseDto dto)
    {
        var expense = await _expenseService.UpdateAsync(id, dto);
        return Ok(expense);
    }

    [HttpPost("{id}/approve")]
    [Authorize(Roles = "Admin,Accounting,StoreManager")]
    public async Task<ActionResult<ExpenseDto>> ApproveExpense(int id, ApproveExpenseDto dto)
    {
        var expense = await _expenseService.ApproveExpenseAsync(id, dto);
        return Ok(expense);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Accounting")]
    public async Task<IActionResult> DeleteExpense(int id)
    {
        await _expenseService.DeleteAsync(id);
        return NoContent();
    }
}
