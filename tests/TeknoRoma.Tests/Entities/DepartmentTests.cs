using TeknoRoma.Domain.Entities;
using Xunit;

namespace TeknoRoma.Tests.Entities;

public class DepartmentTests
{
    [Fact]
    public void Department_Creation_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var department = new Department
        {
            Id = 1,
            DepartmentName = "Information Technology",
            DepartmentCode = "IT-001",
            Description = "IT Department handling all technical operations",
            ManagerId = 5,
            Budget = 500000,
            EmployeeCount = 25,
            IsActive = true,
            CreatedDate = DateTime.Now
        };

        // Assert
        Assert.Equal(1, department.Id);
        Assert.Equal("Information Technology", department.DepartmentName);
        Assert.Equal("IT-001", department.DepartmentCode);
        Assert.Equal("IT Department handling all technical operations", department.Description);
        Assert.Equal(5, department.ManagerId);
        Assert.Equal(500000, department.Budget);
        Assert.Equal(25, department.EmployeeCount);
        Assert.True(department.IsActive);
        Assert.False(department.IsDeleted);
    }

    [Fact]
    public void Department_DefaultValues_ShouldBeSetCorrectly()
    {
        // Arrange & Act
        var department = new Department();

        // Assert
        Assert.Equal(string.Empty, department.DepartmentName);
        Assert.Equal(string.Empty, department.DepartmentCode);
        Assert.True(department.IsActive);
        Assert.False(department.IsDeleted);
        Assert.Null(department.ManagerId);
        Assert.Null(department.Budget);
        Assert.Null(department.EmployeeCount);
    }

    [Fact]
    public void Department_CanHaveManager()
    {
        // Arrange
        var manager = new Employee
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com",
            IdentityNumber = "12345678901",
            Username = "john.doe",
            PasswordHash = "hash"
        };

        var department = new Department
        {
            Id = 1,
            DepartmentName = "Sales",
            DepartmentCode = "SAL-001"
        };

        // Act
        department.ManagerId = manager.Id;
        department.Manager = manager;

        // Assert
        Assert.NotNull(department.Manager);
        Assert.Equal(1, department.ManagerId);
        Assert.Equal("John Doe", $"{department.Manager.FirstName} {department.Manager.LastName}");
    }

    [Fact]
    public void Department_CanHaveMultipleEmployees()
    {
        // Arrange
        var department = new Department
        {
            Id = 1,
            DepartmentName = "Marketing",
            DepartmentCode = "MKT-001"
        };

        var employee1 = new Employee
        {
            Id = 1,
            FirstName = "Alice",
            LastName = "Smith",
            Email = "alice@test.com",
            IdentityNumber = "11111111111",
            Username = "alice",
            PasswordHash = "hash",
            DepartmentId = department.Id
        };

        var employee2 = new Employee
        {
            Id = 2,
            FirstName = "Bob",
            LastName = "Johnson",
            Email = "bob@test.com",
            IdentityNumber = "22222222222",
            Username = "bob",
            PasswordHash = "hash",
            DepartmentId = department.Id
        };

        // Act
        department.Employees = new List<Employee> { employee1, employee2 };

        // Assert
        Assert.NotNull(department.Employees);
        Assert.Equal(2, department.Employees.Count);
    }

    [Theory]
    [InlineData("IT-001")]
    [InlineData("SAL-002")]
    [InlineData("MKT-003")]
    [InlineData("HR-004")]
    public void Department_ShouldAcceptValidDepartmentCodes(string code)
    {
        // Arrange & Act
        var department = new Department { DepartmentCode = code };

        // Assert
        Assert.Equal(code, department.DepartmentCode);
    }

    [Theory]
    [InlineData(50000)]
    [InlineData(100000)]
    [InlineData(500000)]
    [InlineData(1000000)]
    public void Department_ShouldAcceptValidBudgets(decimal budget)
    {
        // Arrange & Act
        var department = new Department { Budget = budget };

        // Assert
        Assert.Equal(budget, department.Budget);
    }

    [Fact]
    public void Department_CanBeActivatedAndDeactivated()
    {
        // Arrange
        var department = new Department
        {
            DepartmentName = "Test Department",
            DepartmentCode = "TST-001",
            IsActive = true
        };

        // Act
        department.IsActive = false;

        // Assert
        Assert.False(department.IsActive);

        // Act again
        department.IsActive = true;

        // Assert
        Assert.True(department.IsActive);
    }

    [Fact]
    public void Department_WithBudgetAndEmployeeCount_ShouldCalculateCorrectly()
    {
        // Arrange & Act
        var department = new Department
        {
            DepartmentName = "Finance",
            DepartmentCode = "FIN-001",
            Budget = 600000,
            EmployeeCount = 20
        };

        var budgetPerEmployee = department.Budget / department.EmployeeCount;

        // Assert
        Assert.Equal(30000, budgetPerEmployee);
    }

    [Fact]
    public void Department_EmployeeCount_ShouldBeUpdatable()
    {
        // Arrange
        var department = new Department
        {
            DepartmentName = "Operations",
            DepartmentCode = "OPS-001",
            EmployeeCount = 10
        };

        // Act - Add employees
        department.EmployeeCount = 15;

        // Assert
        Assert.Equal(15, department.EmployeeCount);

        // Act - Remove employees
        department.EmployeeCount = 12;

        // Assert
        Assert.Equal(12, department.EmployeeCount);
    }

    [Fact]
    public void Department_Budget_ShouldBeUpdatable()
    {
        // Arrange
        var department = new Department
        {
            DepartmentName = "R&D",
            DepartmentCode = "RND-001",
            Budget = 800000
        };

        // Act - Increase budget
        department.Budget = 1000000;

        // Assert
        Assert.Equal(1000000, department.Budget);

        // Act - Decrease budget
        department.Budget = 900000;

        // Assert
        Assert.Equal(900000, department.Budget);
    }

    [Fact]
    public void Department_Manager_CanBeChanged()
    {
        // Arrange
        var oldManager = new Employee
        {
            Id = 1,
            FirstName = "Old",
            LastName = "Manager",
            Email = "old@test.com",
            IdentityNumber = "11111111111",
            Username = "old",
            PasswordHash = "hash"
        };

        var newManager = new Employee
        {
            Id = 2,
            FirstName = "New",
            LastName = "Manager",
            Email = "new@test.com",
            IdentityNumber = "22222222222",
            Username = "new",
            PasswordHash = "hash"
        };

        var department = new Department
        {
            DepartmentName = "Customer Service",
            DepartmentCode = "CS-001",
            ManagerId = oldManager.Id,
            Manager = oldManager
        };

        // Act
        department.ManagerId = newManager.Id;
        department.Manager = newManager;

        // Assert
        Assert.Equal(2, department.ManagerId);
        Assert.Equal("New Manager", $"{department.Manager.FirstName} {department.Manager.LastName}");
    }

    [Theory]
    [InlineData("IT", "Information Technology")]
    [InlineData("HR", "Human Resources")]
    [InlineData("FIN", "Finance")]
    [InlineData("MKT", "Marketing")]
    public void Department_ShouldSupportVariousTypes(string code, string name)
    {
        // Arrange & Act
        var department = new Department
        {
            DepartmentCode = code,
            DepartmentName = name,
            IsActive = true
        };

        // Assert
        Assert.Equal(code, department.DepartmentCode);
        Assert.Equal(name, department.DepartmentName);
        Assert.True(department.IsActive);
    }
}
