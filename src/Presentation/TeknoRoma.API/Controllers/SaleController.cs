using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeknoRoma.Application.DTOs.Sale;
using TeknoRoma.Application.Interfaces.Services;

namespace TeknoRoma.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SaleController : ControllerBase
{
    private readonly ISaleService _saleService;

    public SaleController(ISaleService saleService)
    {
        _saleService = saleService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SaleDto>>> GetAllSales()
    {
        try
        {
            var sales = await _saleService.GetSalesWithDetailsAsync();
            return Ok(sales);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SaleDto>> GetSaleById(int id)
    {
        try
        {
            var sale = await _saleService.GetSaleWithDetailsAsync(id);

            if (sale == null)
            {
                return NotFound($"Sale with ID {id} not found.");
            }

            return Ok(sale);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<SaleDto>>> GetSalesByCustomerId(int customerId)
    {
        try
        {
            var sales = await _saleService.GetSalesByCustomerIdAsync(customerId);
            return Ok(sales);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<IEnumerable<SaleDto>>> GetSalesByEmployeeId(int employeeId)
    {
        try
        {
            var sales = await _saleService.GetSalesByEmployeeIdAsync(employeeId);
            return Ok(sales);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("daterange")]
    public async Task<ActionResult<IEnumerable<SaleDto>>> GetSalesByDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        try
        {
            var sales = await _saleService.GetSalesByDateRangeAsync(startDate, endDate);
            return Ok(sales);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<SaleDto>>> GetSalesByStatus(string status)
    {
        try
        {
            var sales = await _saleService.GetSalesByStatusAsync(status);
            return Ok(sales);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("employee/{employeeId}/totals")]
    public async Task<ActionResult<object>> GetEmployeeSalesTotals(
        int employeeId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        try
        {
            var totalSales = await _saleService.GetTotalSalesAmountByEmployeeAsync(employeeId, startDate, endDate);
            var totalCommission = await _saleService.GetTotalCommissionByEmployeeAsync(employeeId, startDate, endDate);

            return Ok(new
            {
                EmployeeId = employeeId,
                StartDate = startDate,
                EndDate = endDate,
                TotalSalesAmount = totalSales,
                TotalCommissionAmount = totalCommission
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,BranchManager,Cashier")]
    public async Task<ActionResult<SaleDto>> CreateSale([FromBody] CreateSaleDto createSaleDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sale = await _saleService.CreateSaleAsync(createSaleDto);
            return CreatedAtAction(nameof(GetSaleById), new { id = sale.Id }, sale);
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
    [Authorize(Roles = "Admin,BranchManager")]
    public async Task<ActionResult<SaleDto>> UpdateSale(int id, [FromBody] UpdateSaleDto updateSaleDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != updateSaleDto.Id)
            {
                return BadRequest("ID mismatch between route and body.");
            }

            var sale = await _saleService.UpdateSaleAsync(updateSaleDto);
            return Ok(sale);
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
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteSale(int id)
    {
        try
        {
            var result = await _saleService.DeleteSaleAsync(id);

            if (!result)
            {
                return NotFound($"Sale with ID {id} not found.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
