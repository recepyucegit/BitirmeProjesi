using TeknoRoma.Application.DTOs.Expense;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Application.Interfaces.Services;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Services;

public class ExpenseService : IExpenseService
{
    private readonly IUnitOfWork _unitOfWork;

    public ExpenseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ExpenseDto?> GetByIdAsync(int id)
    {
        var expense = await _unitOfWork.Expenses.GetByIdWithDetailsAsync(id);
        if (expense == null) return null;

        return MapToDto(expense);
    }

    public async Task<IEnumerable<ExpenseDto>> GetAllAsync()
    {
        var expenses = await _unitOfWork.Expenses.GetAllWithDetailsAsync();
        return expenses.Select(MapToDto);
    }

    public async Task<IEnumerable<ExpenseDto>> GetByStoreIdAsync(int storeId)
    {
        var expenses = await _unitOfWork.Expenses.GetByStoreIdAsync(storeId);
        return expenses.Select(MapToDto);
    }

    public async Task<IEnumerable<ExpenseDto>> GetByStatusAsync(string status)
    {
        var expenses = await _unitOfWork.Expenses.GetByStatusAsync(status);
        return expenses.Select(MapToDto);
    }

    public async Task<IEnumerable<ExpenseDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var expenses = await _unitOfWork.Expenses.GetByDateRangeAsync(startDate, endDate);
        return expenses.Select(MapToDto);
    }

    public async Task<IEnumerable<ExpenseDto>> GetPendingExpensesAsync()
    {
        var expenses = await _unitOfWork.Expenses.GetPendingExpensesAsync();
        return expenses.Select(MapToDto);
    }

    public async Task<ExpenseDto> CreateAsync(CreateExpenseDto dto)
    {
        // Validate employee if provided
        if (dto.EmployeeId.HasValue)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(dto.EmployeeId.Value);
            if (employee == null)
                throw new InvalidOperationException("Employee not found");
        }

        // Validate store if provided
        if (dto.StoreId.HasValue)
        {
            var store = await _unitOfWork.Stores.GetByIdAsync(dto.StoreId.Value);
            if (store == null)
                throw new InvalidOperationException("Store not found");
        }

        // Calculate amount in TL
        var amountInTL = dto.Currency == "TL" ? dto.Amount : dto.Amount * dto.ExchangeRate;

        var expense = new Expense
        {
            ExpenseType = dto.ExpenseType,
            Description = dto.Description,
            Amount = dto.Amount,
            Currency = dto.Currency,
            ExchangeRate = dto.ExchangeRate,
            AmountInTL = amountInTL,
            ExpenseDate = dto.ExpenseDate,
            EmployeeId = dto.EmployeeId,
            StoreId = dto.StoreId,
            InvoiceNumber = dto.InvoiceNumber,
            Vendor = dto.Vendor,
            Category = dto.Category,
            PaymentMethod = dto.PaymentMethod,
            Status = "Pending",
            Notes = dto.Notes,
            CreatedDate = DateTime.Now
        };

        await _unitOfWork.Expenses.AddAsync(expense);
        await _unitOfWork.SaveChangesAsync();

        var createdExpense = await _unitOfWork.Expenses.GetByIdWithDetailsAsync(expense.Id);
        return MapToDto(createdExpense!);
    }

    public async Task<ExpenseDto> UpdateAsync(int id, UpdateExpenseDto dto)
    {
        var expense = await _unitOfWork.Expenses.GetByIdAsync(id);
        if (expense == null)
            throw new InvalidOperationException("Expense not found");

        // Don't allow update if already approved/rejected
        if (expense.Status == "Approved" || expense.Status == "Rejected")
            throw new InvalidOperationException("Cannot update approved or rejected expense");

        // Validate employee if provided
        if (dto.EmployeeId.HasValue)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(dto.EmployeeId.Value);
            if (employee == null)
                throw new InvalidOperationException("Employee not found");
        }

        // Validate store if provided
        if (dto.StoreId.HasValue)
        {
            var store = await _unitOfWork.Stores.GetByIdAsync(dto.StoreId.Value);
            if (store == null)
                throw new InvalidOperationException("Store not found");
        }

        // Calculate amount in TL
        var amountInTL = dto.Currency == "TL" ? dto.Amount : dto.Amount * dto.ExchangeRate;

        expense.ExpenseType = dto.ExpenseType;
        expense.Description = dto.Description;
        expense.Amount = dto.Amount;
        expense.Currency = dto.Currency;
        expense.ExchangeRate = dto.ExchangeRate;
        expense.AmountInTL = amountInTL;
        expense.ExpenseDate = dto.ExpenseDate;
        expense.EmployeeId = dto.EmployeeId;
        expense.StoreId = dto.StoreId;
        expense.InvoiceNumber = dto.InvoiceNumber;
        expense.Vendor = dto.Vendor;
        expense.Category = dto.Category;
        expense.PaymentMethod = dto.PaymentMethod;
        expense.Status = dto.Status;
        expense.Notes = dto.Notes;
        expense.UpdatedDate = DateTime.Now;

        await _unitOfWork.Expenses.UpdateAsync(expense);
        await _unitOfWork.SaveChangesAsync();

        var updatedExpense = await _unitOfWork.Expenses.GetByIdWithDetailsAsync(id);
        return MapToDto(updatedExpense!);
    }

    public async Task<ExpenseDto> ApproveExpenseAsync(int id, ApproveExpenseDto dto)
    {
        var expense = await _unitOfWork.Expenses.GetByIdAsync(id);
        if (expense == null)
            throw new InvalidOperationException("Expense not found");

        if (expense.Status != "Pending")
            throw new InvalidOperationException("Only pending expenses can be approved/rejected");

        // Validate approver
        var approver = await _unitOfWork.Employees.GetByIdAsync(dto.ApprovedBy);
        if (approver == null)
            throw new InvalidOperationException("Approver not found");

        expense.ApprovedBy = dto.ApprovedBy;
        expense.ApprovalDate = DateTime.Now;
        expense.Status = dto.IsApproved ? "Approved" : "Rejected";

        if (!string.IsNullOrEmpty(dto.Notes))
        {
            expense.Notes = expense.Notes + "\n\nApproval Notes: " + dto.Notes;
        }

        expense.UpdatedDate = DateTime.Now;

        await _unitOfWork.Expenses.UpdateAsync(expense);
        await _unitOfWork.SaveChangesAsync();

        var approvedExpense = await _unitOfWork.Expenses.GetByIdWithDetailsAsync(id);
        return MapToDto(approvedExpense!);
    }

    public async Task DeleteAsync(int id)
    {
        var expense = await _unitOfWork.Expenses.GetByIdAsync(id);
        if (expense == null)
            throw new InvalidOperationException("Expense not found");

        await _unitOfWork.Expenses.DeleteAsync(expense);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<decimal> GetTotalExpensesByStoreAsync(int storeId, DateTime? startDate = null, DateTime? endDate = null)
    {
        return await _unitOfWork.Expenses.GetTotalExpensesByStoreAsync(storeId, startDate, endDate);
    }

    public async Task<decimal> GetTotalExpensesByCategoryAsync(string category, DateTime? startDate = null, DateTime? endDate = null)
    {
        return await _unitOfWork.Expenses.GetTotalExpensesByCategoryAsync(category, startDate, endDate);
    }

    private ExpenseDto MapToDto(Expense expense)
    {
        return new ExpenseDto
        {
            Id = expense.Id,
            ExpenseType = expense.ExpenseType,
            Description = expense.Description,
            Amount = expense.Amount,
            Currency = expense.Currency,
            ExchangeRate = expense.ExchangeRate,
            AmountInTL = expense.AmountInTL,
            ExpenseDate = expense.ExpenseDate,
            EmployeeId = expense.EmployeeId,
            EmployeeName = expense.Employee != null ? $"{expense.Employee.FirstName} {expense.Employee.LastName}" : null,
            StoreId = expense.StoreId,
            StoreName = expense.Store?.StoreName,
            InvoiceNumber = expense.InvoiceNumber,
            Vendor = expense.Vendor,
            Category = expense.Category,
            PaymentMethod = expense.PaymentMethod,
            Status = expense.Status,
            ApprovedBy = expense.ApprovedBy,
            ApproverName = expense.Approver != null ? $"{expense.Approver.FirstName} {expense.Approver.LastName}" : null,
            ApprovalDate = expense.ApprovalDate,
            Notes = expense.Notes,
            CreatedDate = expense.CreatedDate
        };
    }
}
