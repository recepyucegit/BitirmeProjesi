using TeknoRoma.Domain.Entities;
using Xunit;

namespace TeknoRoma.Tests.Entities;

public class UserRoleTests
{
    [Fact]
    public void UserRole_Creation_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var userRole = new UserRole
        {
            Id = 1,
            UserId = 10,
            RoleId = 5,
            CreatedDate = DateTime.Now
        };

        // Assert
        Assert.Equal(1, userRole.Id);
        Assert.Equal(10, userRole.UserId);
        Assert.Equal(5, userRole.RoleId);
        Assert.False(userRole.IsDeleted);
    }

    [Fact]
    public void UserRole_ShouldLinkUserAndRole()
    {
        // Arrange
        var user = new User { Id = 1, Username = "testuser", Email = "test@test.com", PasswordHash = "hash" };
        var role = new Role { Id = 2, Name = "Manager" };

        // Act
        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id,
            User = user,
            Role = role
        };

        // Assert
        Assert.Equal(user.Id, userRole.UserId);
        Assert.Equal(role.Id, userRole.RoleId);
        Assert.NotNull(userRole.User);
        Assert.NotNull(userRole.Role);
        Assert.Equal("testuser", userRole.User.Username);
        Assert.Equal("Manager", userRole.Role.Name);
    }
}
