namespace TeknoRoma.Application.DTOs.User;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public UserDto User { get; set; } = null!;
    public DateTime TokenExpiration { get; set; }
}
