using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeknoRoma.Application.DTOs.Employee;
using TeknoRoma.Application.Interfaces.Services;

namespace TeknoRoma.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetActiveEmployees()
    {
        var employees = await _employeeService.GetActiveEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("role/{role}")]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesByRole(string role)
    {
        var employees = await _employeeService.GetEmployeesByRoleAsync(role);
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        if (employee == null) return NotFound();
        return Ok(employee);
    }

    [HttpGet("username/{username}")]
    public async Task<ActionResult<EmployeeDto>> GetEmployeeByUsername(string username)
    {
        var employee = await _employeeService.GetEmployeeByUsernameAsync(username);
        if (employee == null) return NotFound();
        return Ok(employee);
    }

    [HttpGet("identity/{identityNumber}")]
    public async Task<ActionResult<EmployeeDto>> GetEmployeeByIdentityNumber(string identityNumber)
    {
        var employee = await _employeeService.GetEmployeeByIdentityNumberAsync(identityNumber);
        if (employee == null) return NotFound();
        return Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> CreateEmployee(CreateEmployeeDto dto)
    {
        var employee = await _employeeService.CreateEmployeeAsync(dto);
        return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EmployeeDto>> UpdateEmployee(int id, UpdateEmployeeDto dto)
    {
        if (id != dto.Id) return BadRequest();
        var employee = await _employeeService.UpdateEmployeeAsync(dto);
        return Ok(employee);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var result = await _employeeService.DeleteEmployeeAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpGet("{id}/exists")]
    public async Task<ActionResult<bool>> EmployeeExists(int id)
    {
        var exists = await _employeeService.EmployeeExistsAsync(id);
        return Ok(exists);
    }

    [HttpGet("username/{username}/exists")]
    public async Task<ActionResult<bool>> UsernameExists(string username)
    {
        var exists = await _employeeService.UsernameExistsAsync(username);
        return Ok(exists);
    }

    [HttpGet("identity/{identityNumber}/exists")]
    public async Task<ActionResult<bool>> IdentityNumberExists(string identityNumber)
    {
        var exists = await _employeeService.IdentityNumberExistsAsync(identityNumber);
        return Ok(exists);
    }
}
