using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeknoRoma.Application.DTOs.Role;
using TeknoRoma.Application.Interfaces.Services;

namespace TeknoRoma.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;
    private readonly ILogger<RoleController> _logger;

    public RoleController(IRoleService roleService, ILogger<RoleController> logger)
    {
        _roleService = roleService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all roles");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var role = await _roleService.GetByIdAsync(id);
            if (role == null)
                return NotFound(new { message = "Role not found" });

            return Ok(role);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting role by ID: {Id}", id);
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        try
        {
            var role = await _roleService.GetByNameAsync(name);
            if (role == null)
                return NotFound(new { message = "Role not found" });

            return Ok(role);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting role by name: {Name}", name);
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("name/{name}/exists")]
    public async Task<IActionResult> RoleNameExists(string name)
    {
        try
        {
            var exists = await _roleService.RoleNameExistsAsync(name);
            return Ok(new { exists });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking role name existence");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
    {
        try
        {
            var role = await _roleService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = role.Id }, role);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating role");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleDto dto)
    {
        try
        {
            var role = await _roleService.UpdateAsync(id, dto);
            if (role == null)
                return NotFound(new { message = "Role not found" });

            return Ok(role);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating role: {Id}", id);
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _roleService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting role: {Id}", id);
            return StatusCode(500, new { message = "An error occurred" });
        }
    }
}
