using Microsoft.AspNetCore.Mvc;
using TeknoRoma.Application.DTOs.Supplier;
using TeknoRoma.Application.Interfaces.Services;

namespace TeknoRoma.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SupplierController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SupplierController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupplierDto>>> GetAllSuppliers()
    {
        var suppliers = await _supplierService.GetAllSuppliersAsync();
        return Ok(suppliers);
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<SupplierDto>>> GetActiveSuppliers()
    {
        var suppliers = await _supplierService.GetActiveSuppliersAsync();
        return Ok(suppliers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SupplierDto>> GetSupplier(int id)
    {
        var supplier = await _supplierService.GetSupplierByIdAsync(id);
        if (supplier == null) return NotFound();
        return Ok(supplier);
    }

    [HttpGet("tax/{taxNumber}")]
    public async Task<ActionResult<SupplierDto>> GetSupplierByTaxNumber(string taxNumber)
    {
        var supplier = await _supplierService.GetSupplierByTaxNumberAsync(taxNumber);
        if (supplier == null) return NotFound();
        return Ok(supplier);
    }

    [HttpPost]
    public async Task<ActionResult<SupplierDto>> CreateSupplier(CreateSupplierDto dto)
    {
        var supplier = await _supplierService.CreateSupplierAsync(dto);
        return CreatedAtAction(nameof(GetSupplier), new { id = supplier.Id }, supplier);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SupplierDto>> UpdateSupplier(int id, UpdateSupplierDto dto)
    {
        if (id != dto.Id) return BadRequest();
        var supplier = await _supplierService.UpdateSupplierAsync(dto);
        return Ok(supplier);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
        var result = await _supplierService.DeleteSupplierAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpGet("{id}/exists")]
    public async Task<ActionResult<bool>> SupplierExists(int id)
    {
        var exists = await _supplierService.SupplierExistsAsync(id);
        return Ok(exists);
    }

    [HttpGet("tax/{taxNumber}/exists")]
    public async Task<ActionResult<bool>> TaxNumberExists(string taxNumber)
    {
        var exists = await _supplierService.TaxNumberExistsAsync(taxNumber);
        return Ok(exists);
    }
}
