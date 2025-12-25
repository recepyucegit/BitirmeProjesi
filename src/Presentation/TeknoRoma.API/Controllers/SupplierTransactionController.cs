using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeknoRoma.Application.DTOs.SupplierTransaction;
using TeknoRoma.Application.Interfaces.Services;

namespace TeknoRoma.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SupplierTransactionController : ControllerBase
{
    private readonly ISupplierTransactionService _transactionService;

    public SupplierTransactionController(ISupplierTransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupplierTransactionDto>>> GetAllTransactions()
    {
        try
        {
            var transactions = await _transactionService.GetTransactionsWithDetailsAsync();
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SupplierTransactionDto>> GetTransactionById(int id)
    {
        try
        {
            var transaction = await _transactionService.GetTransactionWithDetailsAsync(id);

            if (transaction == null)
            {
                return NotFound($"SupplierTransaction with ID {id} not found.");
            }

            return Ok(transaction);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("supplier/{supplierId}")]
    public async Task<ActionResult<IEnumerable<SupplierTransactionDto>>> GetTransactionsBySupplierId(int supplierId)
    {
        try
        {
            var transactions = await _transactionService.GetTransactionsBySupplierIdAsync(supplierId);
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<IEnumerable<SupplierTransactionDto>>> GetTransactionsByEmployeeId(int employeeId)
    {
        try
        {
            var transactions = await _transactionService.GetTransactionsByEmployeeIdAsync(employeeId);
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<SupplierTransactionDto>>> GetTransactionsByStatus(string status)
    {
        try
        {
            var transactions = await _transactionService.GetTransactionsByStatusAsync(status);
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("daterange")]
    public async Task<ActionResult<IEnumerable<SupplierTransactionDto>>> GetTransactionsByDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        try
        {
            var transactions = await _transactionService.GetTransactionsByDateRangeAsync(startDate, endDate);
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<SupplierTransactionDto>> CreateTransaction([FromBody] CreateSupplierTransactionDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transaction = await _transactionService.CreateTransactionAsync(createDto);
            return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SupplierTransactionDto>> UpdateTransaction(int id, [FromBody] UpdateSupplierTransactionDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != updateDto.Id)
            {
                return BadRequest("ID mismatch between route and body.");
            }

            var transaction = await _transactionService.UpdateTransactionAsync(updateDto);
            return Ok(transaction);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTransaction(int id)
    {
        try
        {
            var result = await _transactionService.DeleteTransactionAsync(id);

            if (!result)
            {
                return NotFound($"SupplierTransaction with ID {id} not found.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
