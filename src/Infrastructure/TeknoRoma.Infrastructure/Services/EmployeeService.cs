using System.Security.Cryptography;
using System.Text;
using TeknoRoma.Application.DTOs.Employee;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Application.Interfaces.Services;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;

    public EmployeeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private static EmployeeDto MapToDto(Employee employee)
    {
        return new EmployeeDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Phone = employee.Phone,
            Address = employee.Address,
            City = employee.City,
            IdentityNumber = employee.IdentityNumber,
            HireDate = employee.HireDate,
            TerminationDate = employee.TerminationDate,
            Salary = employee.Salary,
            SalesQuota = employee.SalesQuota,
            CommissionRate = employee.CommissionRate,
            Role = employee.Role,
            Username = employee.Username,
            IsActive = employee.IsActive,
            CreatedDate = employee.CreatedDate
        };
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
    {
        var employees = await _unitOfWork.Employees.GetAllAsync();
        return employees.Select(MapToDto);
    }

    public async Task<IEnumerable<EmployeeDto>> GetActiveEmployeesAsync()
    {
        var employees = await _unitOfWork.Employees.GetActiveEmployeesAsync();
        return employees.Select(MapToDto);
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(id);
        return employee == null ? null : MapToDto(employee);
    }

    public async Task<EmployeeDto?> GetEmployeeByUsernameAsync(string username)
    {
        var employee = await _unitOfWork.Employees.GetEmployeeByUsernameAsync(username);
        return employee == null ? null : MapToDto(employee);
    }

    public async Task<EmployeeDto?> GetEmployeeByIdentityNumberAsync(string identityNumber)
    {
        var employee = await _unitOfWork.Employees.GetEmployeeByIdentityNumberAsync(identityNumber);
        return employee == null ? null : MapToDto(employee);
    }

    public async Task<IEnumerable<EmployeeDto>> GetEmployeesByRoleAsync(string role)
    {
        var employees = await _unitOfWork.Employees.GetEmployeesByRoleAsync(role);
        return employees.Select(MapToDto);
    }

    public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto)
    {
        var employee = new Employee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = dto.Address,
            City = dto.City,
            IdentityNumber = dto.IdentityNumber,
            HireDate = dto.HireDate,
            Salary = dto.Salary,
            SalesQuota = dto.SalesQuota,
            CommissionRate = dto.CommissionRate,
            Role = dto.Role,
            Username = dto.Username,
            PasswordHash = HashPassword(dto.Password),
            IsActive = dto.IsActive
        };

        await _unitOfWork.Employees.AddAsync(employee);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(employee);
    }

    public async Task<EmployeeDto> UpdateEmployeeAsync(UpdateEmployeeDto dto)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(dto.Id);
        if (employee == null) throw new Exception("Employee not found");

        employee.FirstName = dto.FirstName;
        employee.LastName = dto.LastName;
        employee.Email = dto.Email;
        employee.Phone = dto.Phone;
        employee.Address = dto.Address;
        employee.City = dto.City;
        employee.IdentityNumber = dto.IdentityNumber;
        employee.HireDate = dto.HireDate;
        employee.TerminationDate = dto.TerminationDate;
        employee.Salary = dto.Salary;
        employee.SalesQuota = dto.SalesQuota;
        employee.CommissionRate = dto.CommissionRate;
        employee.Role = dto.Role;
        employee.Username = dto.Username;
        employee.IsActive = dto.IsActive;

        if (!string.IsNullOrEmpty(dto.Password))
        {
            employee.PasswordHash = HashPassword(dto.Password);
        }

        await _unitOfWork.Employees.UpdateAsync(employee);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(employee);
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(id);
        if (employee == null) return false;

        await _unitOfWork.Employees.DeleteAsync(employee);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EmployeeExistsAsync(int id)
    {
        return await _unitOfWork.Employees.AnyAsync(e => e.Id == id);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _unitOfWork.Employees.UsernameExistsAsync(username);
    }

    public async Task<bool> IdentityNumberExistsAsync(string identityNumber)
    {
        return await _unitOfWork.Employees.IdentityNumberExistsAsync(identityNumber);
    }
}
