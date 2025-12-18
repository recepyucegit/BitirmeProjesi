using TeknoRoma.Application.DTOs.Department;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Application.Interfaces.Services;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public DepartmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DepartmentDto?> GetByIdAsync(int id)
    {
        var department = await _unitOfWork.Departments.GetByIdWithDetailsAsync(id);
        if (department == null) return null;

        return MapToDto(department);
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        var departments = await _unitOfWork.Departments.GetAllWithDetailsAsync();
        return departments.Select(MapToDto);
    }

    public async Task<DepartmentDto?> GetByCodeAsync(string code)
    {
        var department = await _unitOfWork.Departments.GetByCodeAsync(code);
        if (department == null) return null;

        return MapToDto(department);
    }

    public async Task<DepartmentDto?> GetByNameAsync(string name)
    {
        var department = await _unitOfWork.Departments.GetByNameAsync(name);
        if (department == null) return null;

        return MapToDto(department);
    }

    public async Task<IEnumerable<DepartmentDto>> GetByManagerIdAsync(int managerId)
    {
        var departments = await _unitOfWork.Departments.GetByManagerIdAsync(managerId);
        return departments.Select(MapToDto);
    }

    public async Task<IEnumerable<DepartmentDto>> GetActiveDepartmentsAsync()
    {
        var departments = await _unitOfWork.Departments.GetActiveDepartmentsAsync();
        return departments.Select(MapToDto);
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
    {
        // Validate manager if provided
        if (dto.ManagerId.HasValue)
        {
            var manager = await _unitOfWork.Employees.GetByIdAsync(dto.ManagerId.Value);
            if (manager == null)
                throw new InvalidOperationException("Manager not found");
        }

        // Check if department code already exists
        var existingDept = await _unitOfWork.Departments.GetByCodeAsync(dto.DepartmentCode);
        if (existingDept != null)
            throw new InvalidOperationException("Department code already exists");

        var department = new Department
        {
            DepartmentName = dto.DepartmentName,
            DepartmentCode = dto.DepartmentCode,
            Description = dto.Description,
            ManagerId = dto.ManagerId,
            Budget = dto.Budget,
            EmployeeCount = dto.EmployeeCount,
            IsActive = dto.IsActive,
            CreatedDate = DateTime.Now
        };

        await _unitOfWork.Departments.AddAsync(department);
        await _unitOfWork.SaveChangesAsync();

        var createdDepartment = await _unitOfWork.Departments.GetByIdWithDetailsAsync(department.Id);
        return MapToDto(createdDepartment!);
    }

    public async Task<DepartmentDto> UpdateAsync(int id, UpdateDepartmentDto dto)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(id);
        if (department == null)
            throw new InvalidOperationException("Department not found");

        // Validate manager if provided
        if (dto.ManagerId.HasValue)
        {
            var manager = await _unitOfWork.Employees.GetByIdAsync(dto.ManagerId.Value);
            if (manager == null)
                throw new InvalidOperationException("Manager not found");
        }

        // Check if department code already exists (excluding current department)
        var existingDept = await _unitOfWork.Departments.GetByCodeAsync(dto.DepartmentCode);
        if (existingDept != null && existingDept.Id != id)
            throw new InvalidOperationException("Department code already exists");

        department.DepartmentName = dto.DepartmentName;
        department.DepartmentCode = dto.DepartmentCode;
        department.Description = dto.Description;
        department.ManagerId = dto.ManagerId;
        department.Budget = dto.Budget;
        department.EmployeeCount = dto.EmployeeCount;
        department.IsActive = dto.IsActive;
        department.UpdatedDate = DateTime.Now;

        await _unitOfWork.Departments.UpdateAsync(department);
        await _unitOfWork.SaveChangesAsync();

        var updatedDepartment = await _unitOfWork.Departments.GetByIdWithDetailsAsync(id);
        return MapToDto(updatedDepartment!);
    }

    public async Task DeleteAsync(int id)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(id);
        if (department == null)
            throw new InvalidOperationException("Department not found");

        await _unitOfWork.Departments.DeleteAsync(department);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<int> GetTotalEmployeeCountAsync()
    {
        return await _unitOfWork.Departments.GetTotalEmployeeCountAsync();
    }

    public async Task<decimal> GetTotalBudgetAsync()
    {
        return await _unitOfWork.Departments.GetTotalBudgetAsync();
    }

    private DepartmentDto MapToDto(Department department)
    {
        return new DepartmentDto
        {
            Id = department.Id,
            DepartmentName = department.DepartmentName,
            DepartmentCode = department.DepartmentCode,
            Description = department.Description,
            ManagerId = department.ManagerId,
            ManagerName = department.Manager != null ? $"{department.Manager.FirstName} {department.Manager.LastName}" : null,
            Budget = department.Budget,
            EmployeeCount = department.EmployeeCount,
            IsActive = department.IsActive,
            CreatedDate = department.CreatedDate
        };
    }
}
