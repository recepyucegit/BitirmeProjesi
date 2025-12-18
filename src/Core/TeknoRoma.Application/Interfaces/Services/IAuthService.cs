using TeknoRoma.Application.DTOs.User;

namespace TeknoRoma.Application.Interfaces.Services;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginDto dto);
    Task<LoginResponseDto> RefreshTokenAsync(string refreshToken);
    Task<bool> LogoutAsync(int userId);
    string GenerateJwtToken(UserDto user);
    string GenerateRefreshToken();
}
