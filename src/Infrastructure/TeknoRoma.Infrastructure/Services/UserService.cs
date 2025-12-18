using TeknoRoma.Application.DTOs.User;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Application.Interfaces.Services;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _unitOfWork.Users.GetByIdWithRolesAsync(id);
        if (user == null) return null;

        return MapToDto(user);
    }

    public async Task<UserDto?> GetByUsernameAsync(string username)
    {
        var user = await _unitOfWork.Users.GetByUsernameWithRolesAsync(username);
        if (user == null) return null;

        return MapToDto(user);
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _unitOfWork.Users.GetAllWithRolesAsync();
        return users.Select(MapToDto);
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        // Validate username and email uniqueness
        if (await _unitOfWork.Users.UsernameExistsAsync(dto.Username))
            throw new InvalidOperationException("Username already exists");

        if (await _unitOfWork.Users.EmailExistsAsync(dto.Email))
            throw new InvalidOperationException("Email already exists");

        // Create user entity
        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            PhoneNumber = dto.PhoneNumber,
            EmployeeId = dto.EmployeeId,
            IsActive = true,
            CreatedDate = DateTime.Now
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // Add user roles
        if (dto.RoleIds.Any())
        {
            foreach (var roleId in dto.RoleIds)
            {
                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = roleId,
                    CreatedDate = DateTime.Now
                };
                await _unitOfWork.UserRoles.AddAsync(userRole);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        // Return created user with roles
        var createdUser = await _unitOfWork.Users.GetByIdWithRolesAsync(user.Id);
        return MapToDto(createdUser!);
    }

    public async Task<UserDto> UpdateAsync(int id, UpdateUserDto dto)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
            throw new InvalidOperationException("User not found");

        // Check username uniqueness (excluding current user)
        var existingUser = await _unitOfWork.Users.GetByUsernameAsync(dto.Username);
        if (existingUser != null && existingUser.Id != id)
            throw new InvalidOperationException("Username already exists");

        // Check email uniqueness (excluding current user)
        existingUser = await _unitOfWork.Users.GetByEmailAsync(dto.Email);
        if (existingUser != null && existingUser.Id != id)
            throw new InvalidOperationException("Email already exists");

        // Update user properties
        user.Username = dto.Username;
        user.Email = dto.Email;
        user.PhoneNumber = dto.PhoneNumber;
        user.IsActive = dto.IsActive;
        user.EmployeeId = dto.EmployeeId;
        user.UpdatedDate = DateTime.Now;

        await _unitOfWork.Users.UpdateAsync(user);

        // Update user roles
        await _unitOfWork.UserRoles.DeleteByUserIdAsync(id);
        await _unitOfWork.SaveChangesAsync();

        if (dto.RoleIds.Any())
        {
            foreach (var roleId in dto.RoleIds)
            {
                var userRole = new UserRole
                {
                    UserId = id,
                    RoleId = roleId,
                    CreatedDate = DateTime.Now
                };
                await _unitOfWork.UserRoles.AddAsync(userRole);
            }
        }

        await _unitOfWork.SaveChangesAsync();

        // Return updated user with roles
        var updatedUser = await _unitOfWork.Users.GetByIdWithRolesAsync(id);
        return MapToDto(updatedUser!);
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
            throw new InvalidOperationException("User not found");

        await _unitOfWork.Users.DeleteAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _unitOfWork.Users.GetByUsernameWithRolesAsync(dto.Username);
        if (user == null || !user.IsActive)
            throw new UnauthorizedAccessException("Invalid username or password");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid username or password");

        // Update last login date
        user.LastLoginDate = DateTime.Now;
        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // This will be implemented by AuthService
        throw new NotImplementedException("Use AuthService.LoginAsync instead");
    }

    public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto dto)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
            throw new InvalidOperationException("Current password is incorrect");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        user.UpdatedDate = DateTime.Now;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _unitOfWork.Users.UsernameExistsAsync(username);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _unitOfWork.Users.EmailExistsAsync(email);
    }

    private UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            IsActive = user.IsActive,
            LastLoginDate = user.LastLoginDate,
            EmployeeId = user.EmployeeId,
            EmployeeFullName = user.Employee != null ? $"{user.Employee.FirstName} {user.Employee.LastName}" : null,
            Roles = user.UserRoles?.Select(ur => ur.Role?.Name ?? string.Empty).Where(r => !string.IsNullOrEmpty(r)).ToList() ?? new List<string>(),
            CreatedDate = user.CreatedDate
        };
    }
}
