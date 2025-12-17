using TeknoRoma.Application.DTOs.SupplierTransaction;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Application.Interfaces.Services;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Services;

public class SupplierTransactionService : ISupplierTransactionService
{
    private readonly ISupplierTransactionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public SupplierTransactionService(ISupplierTransactionRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<SupplierTransactionDto>> GetAllAsync()
    {
        var transactions = await _repository.GetAllAsync();
        return transactions.Select(MapToDto);
    }

    public async Task<SupplierTransactionDto?> GetByIdAsync(int id)
    {
        var transaction = await _repository.GetByIdAsync(id);
        return transaction != null ? MapToDto(transaction) : null;
    }

    public async Task<IEnumerable<SupplierTransactionDto>> GetBySupplierIdAsync(int supplierId)
    {
        var transactions = await _repository.GetBySupplierIdAsync(supplierId);
        return transactions.Select(MapToDto);
    }

    public async Task<IEnumerable<SupplierTransactionDto>> GetByProductIdAsync(int productId)
    {
        var transactions = await _repository.GetByProductIdAsync(productId);
        return transactions.Select(MapToDto);
    }

    public async Task<IEnumerable<SupplierTransactionDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var transactions = await _repository.GetByDateRangeAsync(startDate, endDate);
        return transactions.Select(MapToDto);
    }

    public async Task<IEnumerable<SupplierTransactionDto>> GetByTransactionTypeAsync(string transactionType)
    {
        var transactions = await _repository.GetByTransactionTypeAsync(transactionType);
        return transactions.Select(MapToDto);
    }

    public async Task<SupplierTransactionDto> CreateAsync(CreateSupplierTransactionDto createDto)
    {
        var transaction = new SupplierTransaction
        {
            SupplierId = createDto.SupplierId,
            ProductId = createDto.ProductId,
            TransactionType = createDto.TransactionType,
            Amount = createDto.Amount,
            Quantity = createDto.Quantity,
            UnitPrice = createDto.UnitPrice,
            Description = createDto.Description,
            TransactionDate = createDto.TransactionDate,
            InvoiceNumber = createDto.InvoiceNumber,
            ReferenceNumber = createDto.ReferenceNumber,
            Status = createDto.Status
        };

        await _repository.AddAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(transaction);
    }

    public async Task<SupplierTransactionDto> UpdateAsync(UpdateSupplierTransactionDto updateDto)
    {
        var transaction = await _repository.GetByIdAsync(updateDto.Id);
        if (transaction == null)
            throw new KeyNotFoundException($"SupplierTransaction with ID {updateDto.Id} not found");

        transaction.SupplierId = updateDto.SupplierId;
        transaction.ProductId = updateDto.ProductId;
        transaction.TransactionType = updateDto.TransactionType;
        transaction.Amount = updateDto.Amount;
        transaction.Quantity = updateDto.Quantity;
        transaction.UnitPrice = updateDto.UnitPrice;
        transaction.Description = updateDto.Description;
        transaction.TransactionDate = updateDto.TransactionDate;
        transaction.InvoiceNumber = updateDto.InvoiceNumber;
        transaction.ReferenceNumber = updateDto.ReferenceNumber;
        transaction.Status = updateDto.Status;

        await _repository.UpdateAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(transaction);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var transaction = await _repository.GetByIdAsync(id);
        if (transaction == null)
            return false;

        await _repository.DeleteAsync(transaction);
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
            ProductId = transaction.ProductId,
            ProductName = transaction.Product?.Name,
            TransactionType = transaction.TransactionType,
            Amount = transaction.Amount,
            Quantity = transaction.Quantity,
            UnitPrice = transaction.UnitPrice,
            Description = transaction.Description,
            TransactionDate = transaction.TransactionDate,
            InvoiceNumber = transaction.InvoiceNumber,
            ReferenceNumber = transaction.ReferenceNumber,
            Status = transaction.Status,
            CreatedDate = transaction.CreatedDate
        };
    }
}
