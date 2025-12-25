using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeknoRoma.Application.DTOs.Customer;
using TeknoRoma.Application.Interfaces.Services;

namespace TeknoRoma.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllCustomers()
    {
        try
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetActiveCustomers()
    {
        try
        {
            var customers = await _customerService.GetActiveCustomersAsync();
            return Ok(customers);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetCustomerById(int id)
    {
        try
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);

            if (customer == null)
            {
                return NotFound($"Customer with ID {id} not found.");
            }

            return Ok(customer);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("identity/{identityNumber}")]
    public async Task<ActionResult<CustomerDto>> GetCustomerByIdentityNumber(string identityNumber)
    {
        try
        {
            var customer = await _customerService.GetCustomerByIdentityNumberAsync(identityNumber);

            if (customer == null)
            {
                return NotFound($"Customer with Identity Number {identityNumber} not found.");
            }

            return Ok(customer);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("type/{customerType}")]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomersByType(string customerType)
    {
        try
        {
            var customers = await _customerService.GetCustomersByTypeAsync(customerType);
            return Ok(customers);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("city/{city}")]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomersByCity(string city)
    {
        try
        {
            var customers = await _customerService.GetCustomersByCityAsync(city);
            return Ok(customers);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer([FromBody] CreateCustomerDto createCustomerDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if identity number already exists
            var exists = await _customerService.IdentityNumberExistsAsync(createCustomerDto.IdentityNumber);
            if (exists)
            {
                return Conflict($"A customer with Identity Number {createCustomerDto.IdentityNumber} already exists.");
            }

            var customer = await _customerService.CreateCustomerAsync(createCustomerDto);
            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerDto>> UpdateCustomer(int id, [FromBody] UpdateCustomerDto updateCustomerDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != updateCustomerDto.Id)
            {
                return BadRequest("ID mismatch between route and body.");
            }

            // Check if identity number is being used by another customer
            var identityExists = await _customerService.IdentityNumberExistsForOtherCustomerAsync(
                updateCustomerDto.IdentityNumber, updateCustomerDto.Id);

            if (identityExists)
            {
                return Conflict($"Identity Number {updateCustomerDto.IdentityNumber} is already in use by another customer.");
            }

            var customer = await _customerService.UpdateCustomerAsync(updateCustomerDto);
            return Ok(customer);
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
    public async Task<ActionResult> DeleteCustomer(int id)
    {
        try
        {
            var result = await _customerService.DeleteCustomerAsync(id);

            if (!result)
            {
                return NotFound($"Customer with ID {id} not found.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("exists/identity/{identityNumber}")]
    public async Task<ActionResult<bool>> CheckIdentityNumberExists(string identityNumber)
    {
        try
        {
            var exists = await _customerService.IdentityNumberExistsAsync(identityNumber);
            return Ok(exists);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
