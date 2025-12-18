using TeknoRoma.Application.DTOs.Role;

namespace TeknoRoma.Application.Interfaces.Services;

public interface IRoleService
{
    Task<RoleDto?> GetByIdAsync(int id);
    Task<RoleDto?> GetByNameAsync(string name);
    Task<IEnumerable<RoleDto>> GetAllAsync();
    Task<RoleDto> CreateAsync(CreateRoleDto dto);
    Task<RoleDto> UpdateAsync(int id, UpdateRoleDto dto);
    Task DeleteAsync(int id);
    Task<bool> RoleNameExistsAsync(string name);
}
