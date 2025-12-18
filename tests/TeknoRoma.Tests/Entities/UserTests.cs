using TeknoRoma.Domain.Entities;
using Xunit;

namespace TeknoRoma.Tests.Entities;

public class UserTests
{
    [Fact]
    public void User_Creation_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@teknorma.com",
            PasswordHash = "hashedpassword123",
            PhoneNumber = "5551234567",
            IsActive = true,
            EmployeeId = 1,
            CreatedDate = DateTime.Now
        };

        // Assert
        Assert.Equal(1, user.Id);
        Assert.Equal("testuser", user.Username);
        Assert.Equal("test@teknorma.com", user.Email);
        Assert.Equal("hashedpassword123", user.PasswordHash);
        Assert.Equal("5551234567", user.PhoneNumber);
        Assert.True(user.IsActive);
        Assert.Equal(1, user.EmployeeId);
        Assert.False(user.IsDeleted);
    }

    [Fact]
    public void User_DefaultValues_ShouldBeSetCorrectly()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Equal(string.Empty, user.Username);
        Assert.Equal(string.Empty, user.Email);
        Assert.Equal(string.Empty, user.PasswordHash);
        Assert.True(user.IsActive);
        Assert.False(user.IsDeleted);
        Assert.Null(user.EmployeeId);
        Assert.Null(user.LastLoginDate);
    }

    [Fact]
    public void User_RefreshToken_ShouldBeNullableAndSettable()
    {
        // Arrange
        var user = new User();
        var refreshToken = "test-refresh-token-12345";
        var expiryTime = DateTime.Now.AddDays(7);

        // Act
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = expiryTime;

        // Assert
        Assert.Equal(refreshToken, user.RefreshToken);
        Assert.Equal(expiryTime, user.RefreshTokenExpiryTime);
    }

    [Fact]
    public void User_CanHaveMultipleRoles()
    {
        // Arrange
        var user = new User { Id = 1, Username = "testuser", Email = "test@test.com", PasswordHash = "hash" };
        var role1 = new Role { Id = 1, Name = "Admin" };
        var role2 = new Role { Id = 2, Name = "Manager" };

        // Act
        user.UserRoles = new List<UserRole>
        {
            new UserRole { UserId = user.Id, RoleId = role1.Id, Role = role1 },
            new UserRole { UserId = user.Id, RoleId = role2.Id, Role = role2 }
        };

        // Assert
        Assert.NotNull(user.UserRoles);
        Assert.Equal(2, user.UserRoles.Count);
    }
}
