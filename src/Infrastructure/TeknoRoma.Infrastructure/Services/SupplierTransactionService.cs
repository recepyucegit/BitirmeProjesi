using TeknoRoma.Application.DTOs.SupplierTransaction;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Application.Interfaces.Services;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Services;

public class SupplierTransactionService : ISupplierTransactionService
{
    private readonly IUnitOfWork _unitOfWork;

    public SupplierTransactionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<SupplierTransactionDto>> GetAllTransactionsAsync()
    {
        var transactions = await _unitOfWork.SupplierTransactions.GetAllAsync();
        return transactions.Select(MapToDto);
    }

    public async Task<IEnumerable<SupplierTransactionDto>> GetTransactionsWithDetailsAsync()
    {
        var transactions = await _unitOfWork.SupplierTransactions.GetTransactionsWithDetailsAsync();
        return transactions.Select(MapToDtoWithDetails);
    }

    public async Task<SupplierTransactionDto?> GetTransactionByIdAsync(int id)
    {
        var transaction = await _unitOfWork.SupplierTransactions.GetByIdAsync(id);
        return transaction == null ? null : MapToDto(transaction);
    }

    public async Task<SupplierTransactionDto?> GetTransactionWithDetailsAsync(int id)
    {
        var transaction = await _unitOfWork.SupplierTransactions.GetTransactionWithDetailsAsync(id);
        return transaction == null ? null : MapToDtoWithDetails(transaction);
    }

    public async Task<IEnumerable<SupplierTransactionDto>> GetTransactionsBySupplierIdAsync(int supplierId)
    {
        var transactions = await _unitOfWork.SupplierTransactions.GetTransactionsBySupplierIdAsync(supplierId);
        return transactions.Select(MapToDtoWithDetails);
    }

    public async Task<IEnumerable<SupplierTransactionDto>> GetTransactionsByEmployeeIdAsync(int employeeId)
    {
        var transactions = await _unitOfWork.SupplierTransactions.GetTransactionsByEmployeeIdAsync(employeeId);
        return transactions.Select(MapToDtoWithDetails);
    }

    public async Task<IEnumerable<SupplierTransactionDto>> GetTransactionsByStatusAsync(string status)
    {
        var transactions = await _unitOfWork.SupplierTransactions.GetTransactionsByStatusAsync(status);
        return transactions.Select(MapToDtoWithDetails);
    }

    public async Task<IEnumerable<SupplierTransactionDto>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var transactions = await _unitOfWork.SupplierTransactions.GetTransactionsByDateRangeAsync(startDate, endDate);
        return transactions.Select(MapToDtoWithDetails);
    }

    public async Task<SupplierTransactionDto> CreateTransactionAsync(CreateSupplierTransactionDto createDto)
    {
        // Validate supplier
        var supplier = await _unitOfWork.Suppliers.GetByIdAsync(createDto.SupplierId);
        if (supplier == null)
        {
            throw new KeyNotFoundException($"Supplier with ID {createDto.SupplierId} not found.");
        }

        // Validate employee
        var employee = await _unitOfWork.Employees.GetByIdAsync(createDto.EmployeeId);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {createDto.EmployeeId} not found.");
        }

        // Generate order number
        var orderNumber = $"ORD-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper()}";

        // Calculate total
        decimal totalAmount = 0;
        var details = new List<SupplierTransactionDetail>();

        foreach (var detailDto in createDto.Details)
        {
            // Validate product
            var product = await _unitOfWork.Products.GetByIdAsync(detailDto.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {detailDto.ProductId} not found.");
            }

            var totalPrice = detailDto.Quantity * detailDto.UnitPrice;

            var detail = new SupplierTransactionDetail
            {
                ProductId = detailDto.ProductId,
                Quantity = detailDto.Quantity,
                UnitPrice = detailDto.UnitPrice,
                TotalPrice = totalPrice
            };

            details.Add(detail);
            totalAmount += totalPrice;
        }

        var transaction = new SupplierTransaction
        {
            SupplierId = createDto.SupplierId,
            EmployeeId = createDto.EmployeeId,
            OrderDate = createDto.OrderDate,
            TotalAmount = totalAmount,
            Status = "Ordered",
            Notes = createDto.Notes,
            OrderNumber = orderNumber,
            Details = details
        };

        await _unitOfWork.SupplierTransactions.AddAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        // Reload with details
        var createdTransaction = await _unitOfWork.SupplierTransactions.GetTransactionWithDetailsAsync(transaction.Id);
        return MapToDtoWithDetails(createdTransaction!);
    }

    public async Task<SupplierTransactionDto> UpdateTransactionAsync(UpdateSupplierTransactionDto updateDto)
    {
        var transaction = await _unitOfWork.SupplierTransactions.GetByIdAsync(updateDto.Id);

        if (transaction == null)
        {
            throw new KeyNotFoundException($"SupplierTransaction with ID {updateDto.Id} not found.");
        }

        // Store old status before updating
        var oldStatus = transaction.Status;

        transaction.Status = updateDto.Status;
        transaction.DeliveryDate = updateDto.DeliveryDate;
        transaction.Notes = updateDto.Notes;

        // If status changed to Delivered, update product stocks
        if (updateDto.Status == "Delivered" && transaction.Status != "Delivered")
        {
            var transactionWithDetails = await _unitOfWork.SupplierTransactions.GetTransactionWithDetailsAsync(updateDto.Id);
            if (transactionWithDetails?.Details != null)
            {
                foreach (var detail in transactionWithDetails.Details)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(detail.ProductId);
                    if (product != null)
                    {
                        product.StockQuantity += detail.Quantity;
                        await _unitOfWork.Products.UpdateAsync(product);
                    }
                }
            }
        }

        await _unitOfWork.SupplierTransactions.UpdateAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(transaction);
    }

    public async Task<bool> DeleteTransactionAsync(int id)
    {
        var transaction = await _unitOfWork.SupplierTransactions.GetByIdAsync(id);

        if (transaction == null)
        {
            return false;
        }

        await _unitOfWork.SupplierTransactions.DeleteAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    private static SupplierTransactionDto MapToDto(SupplierTransaction transaction)
    {
        return new SupplierTransactionDto
        {
            Id = transaction.Id,
            SupplierId = transaction.SupplierId,
            SupplierName = transaction.Supplier?.CompanyName ?? string.Empty,
            EmployeeId = transaction.EmployeeId,
            EmployeeName = transaction.Employee != null ? $"{transaction.Employee.FirstName} {transaction.Employee.LastName}" : string.Empty,
            OrderDate = transaction.OrderDate,
            DeliveryDate = transaction.DeliveryDate,
            TotalAmount = transaction.TotalAmount,
            Status = transaction.Status,
            Notes = transaction.Notes,
            OrderNumber = transaction.OrderNumber,
            CreatedDate = transaction.CreatedDate,
            UpdatedDate = transaction.UpdatedDate
        };
    }

    private static SupplierTransactionDto MapToDtoWithDetails(SupplierTransaction transaction)
    {
        var dto = MapToDto(transaction);

        if (transaction.Details != null)
        {
            dto.Details = transaction.Details.Select(d => new SupplierTransactionDetailDto
            {
                Id = d.Id,
                SupplierTransactionId = d.SupplierTransactionId,
                ProductId = d.ProductId,
                ProductName = d.Product?.Name ?? string.Empty,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice,
                TotalPrice = d.TotalPrice
            }).ToList();
        }

        return dto;
    }
}
