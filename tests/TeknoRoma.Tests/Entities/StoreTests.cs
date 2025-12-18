using TeknoRoma.Domain.Entities;
using Xunit;

namespace TeknoRoma.Tests.Entities;

public class StoreTests
{
    [Fact]
    public void Store_Creation_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var store = new Store
        {
            Id = 1,
            StoreName = "TeknoRoma Kadıköy",
            StoreCode = "TR-KDK-001",
            Address = "Kadıköy Caddesi No:123",
            City = "Istanbul",
            District = "Kadıköy",
            Phone = "02161234567",
            Email = "kadikoy@teknorma.com",
            ManagerId = 5,
            OpeningDate = new DateTime(2020, 1, 15),
            MonthlyTarget = 500000,
            Capacity = 250,
            IsActive = true,
            CreatedDate = DateTime.Now
        };

        // Assert
        Assert.Equal(1, store.Id);
        Assert.Equal("TeknoRoma Kadıköy", store.StoreName);
        Assert.Equal("TR-KDK-001", store.StoreCode);
        Assert.Equal("Kadıköy Caddesi No:123", store.Address);
        Assert.Equal("Istanbul", store.City);
        Assert.Equal("Kadıköy", store.District);
        Assert.Equal("02161234567", store.Phone);
        Assert.Equal("kadikoy@teknorma.com", store.Email);
        Assert.Equal(5, store.ManagerId);
        Assert.Equal(new DateTime(2020, 1, 15), store.OpeningDate);
        Assert.Equal(500000, store.MonthlyTarget);
        Assert.Equal(250, store.Capacity);
        Assert.True(store.IsActive);
        Assert.False(store.IsDeleted);
    }

    [Fact]
    public void Store_DefaultValues_ShouldBeSetCorrectly()
    {
        // Arrange & Act
        var store = new Store();

        // Assert
        Assert.Equal(string.Empty, store.StoreName);
        Assert.Equal(string.Empty, store.StoreCode);
        Assert.True(store.IsActive);
        Assert.False(store.IsDeleted);
        Assert.Null(store.ManagerId);
        Assert.Null(store.MonthlyTarget);
        Assert.Null(store.Capacity);
    }

    [Fact]
    public void Store_CanHaveManager()
    {
        // Arrange
        var manager = new Employee
        {
            Id = 1,
            FirstName = "Ahmet",
            LastName = "Yılmaz",
            Email = "ahmet@test.com",
            IdentityNumber = "12345678901",
            Username = "ahmet.yilmaz",
            PasswordHash = "hash"
        };
        var store = new Store
        {
            Id = 1,
            StoreName = "Test Store",
            StoreCode = "TS-001"
        };

        // Act
        store.ManagerId = manager.Id;
        store.Manager = manager;

        // Assert
        Assert.NotNull(store.Manager);
        Assert.Equal(1, store.ManagerId);
        Assert.Equal("Ahmet Yılmaz", $"{store.Manager.FirstName} {store.Manager.LastName}");
    }

    [Fact]
    public void Store_CanHaveMultipleEmployees()
    {
        // Arrange
        var store = new Store { Id = 1, StoreName = "Test Store", StoreCode = "TS-001" };
        var employee1 = new Employee { Id = 1, FirstName = "Ali", LastName = "Veli", Email = "ali@test.com", IdentityNumber = "11111111111", Username = "ali", PasswordHash = "hash", StoreId = store.Id };
        var employee2 = new Employee { Id = 2, FirstName = "Ayşe", LastName = "Fatma", Email = "ayse@test.com", IdentityNumber = "22222222222", Username = "ayse", PasswordHash = "hash", StoreId = store.Id };

        // Act
        store.Employees = new List<Employee> { employee1, employee2 };

        // Assert
        Assert.NotNull(store.Employees);
        Assert.Equal(2, store.Employees.Count);
    }

    [Fact]
    public void Store_CanHaveMultipleSales()
    {
        // Arrange
        var store = new Store { Id = 1, StoreName = "Test Store", StoreCode = "TS-001" };
        var sale1 = new Sale { Id = 1, CustomerId = 1, EmployeeId = 1, StoreId = store.Id, TotalAmount = 1000, NetAmount = 1000, InvoiceNumber = "INV001" };
        var sale2 = new Sale { Id = 2, CustomerId = 2, EmployeeId = 2, StoreId = store.Id, TotalAmount = 2000, NetAmount = 2000, InvoiceNumber = "INV002" };

        // Act
        store.Sales = new List<Sale> { sale1, sale2 };

        // Assert
        Assert.NotNull(store.Sales);
        Assert.Equal(2, store.Sales.Count);
    }

    [Theory]
    [InlineData("TR-IST-001")]
    [InlineData("TR-ANK-002")]
    [InlineData("TR-IZM-003")]
    public void Store_ShouldAcceptValidStoreCodes(string storeCode)
    {
        // Arrange & Act
        var store = new Store { StoreCode = storeCode };

        // Assert
        Assert.Equal(storeCode, store.StoreCode);
    }
}
