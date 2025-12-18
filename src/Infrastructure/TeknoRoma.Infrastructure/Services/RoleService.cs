using TeknoRoma.Application.DTOs.Role;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Application.Interfaces.Services;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Services;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;

    public RoleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<RoleDto?> GetByIdAsync(int id)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(id);
        if (role == null) return null;

        return MapToDto(role);
    }

    public async Task<RoleDto?> GetByNameAsync(string name)
    {
        var role = await _unitOfWork.Roles.GetByNameAsync(name);
        if (role == null) return null;

        return MapToDto(role);
    }

    public async Task<IEnumerable<RoleDto>> GetAllAsync()
    {
        var roles = await _unitOfWork.Roles.GetAllAsync();
        return roles.Select(MapToDto);
    }

    public async Task<RoleDto> CreateAsync(CreateRoleDto dto)
    {
        // Validate role name uniqueness
        if (await _unitOfWork.Roles.RoleNameExistsAsync(dto.Name))
            throw new InvalidOperationException("Role name already exists");

        var role = new Role
        {
            Name = dto.Name,
            Description = dto.Description,
            IsActive = true,
            CreatedDate = DateTime.Now
        };

        await _unitOfWork.Roles.AddAsync(role);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(role);
    }

    public async Task<RoleDto> UpdateAsync(int id, UpdateRoleDto dto)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(id);
        if (role == null)
            throw new InvalidOperationException("Role not found");

        // Check role name uniqueness (excluding current role)
        var existingRole = await _unitOfWork.Roles.GetByNameAsync(dto.Name);
        if (existingRole != null && existingRole.Id != id)
            throw new InvalidOperationException("Role name already exists");

        role.Name = dto.Name;
        role.Description = dto.Description;
        role.IsActive = dto.IsActive;
        role.UpdatedDate = DateTime.Now;

        await _unitOfWork.Roles.UpdateAsync(role);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(role);
    }

    public async Task DeleteAsync(int id)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(id);
        if (role == null)
            throw new InvalidOperationException("Role not found");

        await _unitOfWork.Roles.DeleteAsync(role);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<bool> RoleNameExistsAsync(string name)
    {
        return await _unitOfWork.Roles.RoleNameExistsAsync(name);
    }

    private RoleDto MapToDto(Role role)
    {
        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            IsActive = role.IsActive,
            CreatedDate = role.CreatedDate
        };
    }
}
