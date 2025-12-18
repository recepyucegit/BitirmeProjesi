using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TeknoRoma.Application.DTOs.User;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Application.Interfaces.Services;

namespace TeknoRoma.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _unitOfWork.Users.GetByUsernameWithRolesAsync(dto.Username);
        if (user == null || !user.IsActive)
            throw new UnauthorizedAccessException("Invalid username or password");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid username or password");

        // Map to UserDto
        var userDto = new UserDto
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

        // Generate tokens
        var token = GenerateJwtToken(userDto);
        var refreshToken = GenerateRefreshToken();

        // Update user with refresh token
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        user.LastLoginDate = DateTime.Now;
        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return new LoginResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            User = userDto,
            TokenExpiration = DateTime.Now.AddHours(24)
        };
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken)
    {
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        if (user == null || user.RefreshTokenExpiryTime < DateTime.Now)
            throw new UnauthorizedAccessException("Invalid or expired refresh token");

        // Get user with roles
        var userWithRoles = await _unitOfWork.Users.GetByIdWithRolesAsync(user.Id);

        var userDto = new UserDto
        {
            Id = userWithRoles!.Id,
            Username = userWithRoles.Username,
            Email = userWithRoles.Email,
            PhoneNumber = userWithRoles.PhoneNumber,
            IsActive = userWithRoles.IsActive,
            LastLoginDate = userWithRoles.LastLoginDate,
            EmployeeId = userWithRoles.EmployeeId,
            EmployeeFullName = userWithRoles.Employee != null ? $"{userWithRoles.Employee.FirstName} {userWithRoles.Employee.LastName}" : null,
            Roles = userWithRoles.UserRoles?.Select(ur => ur.Role?.Name ?? string.Empty).Where(r => !string.IsNullOrEmpty(r)).ToList() ?? new List<string>(),
            CreatedDate = userWithRoles.CreatedDate
        };

        var newToken = GenerateJwtToken(userDto);
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return new LoginResponseDto
        {
            Token = newToken,
            RefreshToken = newRefreshToken,
            User = userDto,
            TokenExpiration = DateTime.Now.AddHours(24)
        };
    }

    public async Task<bool> LogoutAsync(int userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            return false;

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public string GenerateJwtToken(UserDto user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "TeknoRoma_Super_Secret_Key_12345678901234567890"));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
        };

        // Add role claims
        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        if (user.EmployeeId.HasValue)
        {
            claims.Add(new Claim("EmployeeId", user.EmployeeId.Value.ToString()));
        }

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "TeknoRoma",
            audience: _configuration["Jwt:Audience"] ?? "TeknoRoma",
            claims: claims,
            expires: DateTime.Now.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
