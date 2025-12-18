using TeknoRoma.Domain.Entities;
using Xunit;

namespace TeknoRoma.Tests.Entities;

public class ExpenseTests
{
    [Fact]
    public void Expense_Creation_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var expense = new Expense
        {
            Id = 1,
            ExpenseType = "Operational",
            Description = "Office supplies purchase",
            Amount = 500.00m,
            Currency = "USD",
            ExchangeRate = 30.50m,
            AmountInTL = 15250.00m,
            ExpenseDate = new DateTime(2025, 12, 15),
            EmployeeId = 5,
            StoreId = 3,
            InvoiceNumber = "INV-2025-001",
            Vendor = "Office Depot",
            Category = "Supplies",
            PaymentMethod = "BankTransfer",
            Status = "Pending",
            Notes = "Urgent office supplies",
            CreatedDate = DateTime.Now
        };

        // Assert
        Assert.Equal(1, expense.Id);
        Assert.Equal("Operational", expense.ExpenseType);
        Assert.Equal("Office supplies purchase", expense.Description);
        Assert.Equal(500.00m, expense.Amount);
        Assert.Equal("USD", expense.Currency);
        Assert.Equal(30.50m, expense.ExchangeRate);
        Assert.Equal(15250.00m, expense.AmountInTL);
        Assert.Equal(new DateTime(2025, 12, 15), expense.ExpenseDate);
        Assert.Equal(5, expense.EmployeeId);
        Assert.Equal(3, expense.StoreId);
        Assert.Equal("INV-2025-001", expense.InvoiceNumber);
        Assert.Equal("Office Depot", expense.Vendor);
        Assert.Equal("Supplies", expense.Category);
        Assert.Equal("BankTransfer", expense.PaymentMethod);
        Assert.Equal("Pending", expense.Status);
        Assert.Equal("Urgent office supplies", expense.Notes);
        Assert.False(expense.IsDeleted);
    }

    [Fact]
    public void Expense_DefaultValues_ShouldBeSetCorrectly()
    {
        // Arrange & Act
        var expense = new Expense();

        // Assert
        Assert.Equal(string.Empty, expense.ExpenseType);
        Assert.Equal(string.Empty, expense.Description);
        Assert.Equal(0, expense.Amount);
        Assert.Equal("TL", expense.Currency);
        Assert.Equal(1, expense.ExchangeRate);
        Assert.Equal(0, expense.AmountInTL);
        Assert.Equal(string.Empty, expense.Category);
        Assert.Equal("BankTransfer", expense.PaymentMethod);
        Assert.Equal("Pending", expense.Status);
        Assert.False(expense.IsDeleted);
        Assert.Null(expense.EmployeeId);
        Assert.Null(expense.StoreId);
        Assert.Null(expense.ApprovedBy);
        Assert.Null(expense.ApprovalDate);
    }

    [Fact]
    public void Expense_CanBeApproved()
    {
        // Arrange
        var expense = new Expense
        {
            Id = 1,
            ExpenseType = "Operational",
            Description = "Test expense",
            Amount = 1000,
            Status = "Pending"
        };

        var approver = new Employee
        {
            Id = 10,
            FirstName = "Manager",
            LastName = "Smith",
            Email = "manager@test.com",
            IdentityNumber = "12345678901",
            Username = "manager",
            PasswordHash = "hash"
        };

        // Act
        expense.Status = "Approved";
        expense.ApprovedBy = approver.Id;
        expense.ApprovalDate = DateTime.Now;

        // Assert
        Assert.Equal("Approved", expense.Status);
        Assert.Equal(10, expense.ApprovedBy);
        Assert.NotNull(expense.ApprovalDate);
    }

    [Fact]
    public void Expense_CanBeRejected()
    {
        // Arrange
        var expense = new Expense
        {
            Id = 1,
            ExpenseType = "Operational",
            Description = "Test expense",
            Amount = 1000,
            Status = "Pending"
        };

        var approver = new Employee
        {
            Id = 10,
            FirstName = "Manager",
            LastName = "Smith",
            Email = "manager@test.com",
            IdentityNumber = "12345678901",
            Username = "manager",
            PasswordHash = "hash"
        };

        // Act
        expense.Status = "Rejected";
        expense.ApprovedBy = approver.Id;
        expense.ApprovalDate = DateTime.Now;
        expense.Notes = "Budget exceeded";

        // Assert
        Assert.Equal("Rejected", expense.Status);
        Assert.Equal(10, expense.ApprovedBy);
        Assert.NotNull(expense.ApprovalDate);
        Assert.Equal("Budget exceeded", expense.Notes);
    }

    [Fact]
    public void Expense_CanHaveEmployeeRelation()
    {
        // Arrange
        var employee = new Employee
        {
            Id = 5,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@test.com",
            IdentityNumber = "12345678901",
            Username = "john.doe",
            PasswordHash = "hash"
        };

        var expense = new Expense
        {
            Id = 1,
            ExpenseType = "Travel",
            Description = "Business trip",
            Amount = 2000,
            EmployeeId = employee.Id
        };

        // Act
        expense.Employee = employee;

        // Assert
        Assert.NotNull(expense.Employee);
        Assert.Equal(5, expense.EmployeeId);
        Assert.Equal("John Doe", $"{expense.Employee.FirstName} {expense.Employee.LastName}");
    }

    [Fact]
    public void Expense_CanHaveStoreRelation()
    {
        // Arrange
        var store = new Store
        {
            Id = 3,
            StoreName = "TeknoRoma Kadıköy",
            StoreCode = "TR-KDK-001"
        };

        var expense = new Expense
        {
            Id = 1,
            ExpenseType = "Maintenance",
            Description = "Store repairs",
            Amount = 5000,
            StoreId = store.Id
        };

        // Act
        expense.Store = store;

        // Assert
        Assert.NotNull(expense.Store);
        Assert.Equal(3, expense.StoreId);
        Assert.Equal("TeknoRoma Kadıköy", expense.Store.StoreName);
    }

    [Fact]
    public void Expense_CanHaveApproverRelation()
    {
        // Arrange
        var approver = new Employee
        {
            Id = 10,
            FirstName = "Manager",
            LastName = "Boss",
            Email = "manager@test.com",
            IdentityNumber = "12345678901",
            Username = "manager",
            PasswordHash = "hash"
        };

        var expense = new Expense
        {
            Id = 1,
            ExpenseType = "Operational",
            Description = "Test expense",
            Amount = 1000,
            Status = "Approved",
            ApprovedBy = approver.Id,
            ApprovalDate = DateTime.Now
        };

        // Act
        expense.Approver = approver;

        // Assert
        Assert.NotNull(expense.Approver);
        Assert.Equal(10, expense.ApprovedBy);
        Assert.Equal("Manager Boss", $"{expense.Approver.FirstName} {expense.Approver.LastName}");
    }

    [Theory]
    [InlineData("TL", 1000, 1, 1000)]
    [InlineData("USD", 100, 30, 3000)]
    [InlineData("EUR", 200, 35, 7000)]
    public void Expense_AmountInTL_ShouldBeCalculatedCorrectly(string currency, decimal amount, decimal rate, decimal expectedTL)
    {
        // Arrange & Act
        var expense = new Expense
        {
            Currency = currency,
            Amount = amount,
            ExchangeRate = rate,
            AmountInTL = amount * rate
        };

        // Assert
        Assert.Equal(expectedTL, expense.AmountInTL);
    }

    [Theory]
    [InlineData("Pending")]
    [InlineData("Approved")]
    [InlineData("Rejected")]
    [InlineData("Paid")]
    public void Expense_ShouldAcceptValidStatuses(string status)
    {
        // Arrange & Act
        var expense = new Expense { Status = status };

        // Assert
        Assert.Equal(status, expense.Status);
    }

    [Theory]
    [InlineData("BankTransfer")]
    [InlineData("Cash")]
    [InlineData("CreditCard")]
    [InlineData("Check")]
    public void Expense_ShouldAcceptValidPaymentMethods(string paymentMethod)
    {
        // Arrange & Act
        var expense = new Expense { PaymentMethod = paymentMethod };

        // Assert
        Assert.Equal(paymentMethod, expense.PaymentMethod);
    }

    [Theory]
    [InlineData("Operational")]
    [InlineData("Maintenance")]
    [InlineData("Marketing")]
    [InlineData("Travel")]
    [InlineData("Utility")]
    public void Expense_ShouldAcceptValidExpenseTypes(string expenseType)
    {
        // Arrange & Act
        var expense = new Expense { ExpenseType = expenseType };

        // Assert
        Assert.Equal(expenseType, expense.ExpenseType);
    }

    [Fact]
    public void Expense_WithMultipleCurrencies_ShouldTrackCorrectly()
    {
        // Arrange & Act
        var expenseUSD = new Expense { Currency = "USD", Amount = 100, ExchangeRate = 30, AmountInTL = 3000 };
        var expenseEUR = new Expense { Currency = "EUR", Amount = 100, ExchangeRate = 35, AmountInTL = 3500 };
        var expenseTL = new Expense { Currency = "TL", Amount = 3000, ExchangeRate = 1, AmountInTL = 3000 };

        // Assert
        Assert.Equal(3000, expenseUSD.AmountInTL);
        Assert.Equal(3500, expenseEUR.AmountInTL);
        Assert.Equal(3000, expenseTL.AmountInTL);
    }
}
