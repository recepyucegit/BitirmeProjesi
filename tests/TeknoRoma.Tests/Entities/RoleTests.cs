using TeknoRoma.Domain.Entities;
using Xunit;

namespace TeknoRoma.Tests.Entities;

public class RoleTests
{
    [Fact]
    public void Role_Creation_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var role = new Role
        {
            Id = 1,
            Name = "Admin",
            Description = "System Administrator",
            IsActive = true,
            CreatedDate = DateTime.Now
        };

        // Assert
        Assert.Equal(1, role.Id);
        Assert.Equal("Admin", role.Name);
        Assert.Equal("System Administrator", role.Description);
        Assert.True(role.IsActive);
        Assert.False(role.IsDeleted);
    }

    [Fact]
    public void Role_DefaultValues_ShouldBeSetCorrectly()
    {
        // Arrange & Act
        var role = new Role();

        // Assert
        Assert.Equal(string.Empty, role.Name);
        Assert.True(role.IsActive);
        Assert.False(role.IsDeleted);
    }

    [Fact]
    public void Role_CanHaveMultipleUsers()
    {
        // Arrange
        var role = new Role { Id = 1, Name = "Manager" };
        var user1 = new User { Id = 1, Username = "user1", Email = "user1@test.com", PasswordHash = "hash1" };
        var user2 = new User { Id = 2, Username = "user2", Email = "user2@test.com", PasswordHash = "hash2" };

        // Act
        role.UserRoles = new List<UserRole>
        {
            new UserRole { UserId = user1.Id, RoleId = role.Id, User = user1 },
            new UserRole { UserId = user2.Id, RoleId = role.Id, User = user2 }
        };

        // Assert
        Assert.NotNull(role.UserRoles);
        Assert.Equal(2, role.UserRoles.Count);
    }

    [Theory]
    [InlineData("Admin")]
    [InlineData("BranchManager")]
    [InlineData("Cashier")]
    [InlineData("Warehouse")]
    [InlineData("Accounting")]
    [InlineData("TechnicalService")]
    public void Role_ShouldAcceptValidRoleNames(string roleName)
    {
        // Arrange & Act
        var role = new Role { Name = roleName };

        // Assert
        Assert.Equal(roleName, role.Name);
    }
}
