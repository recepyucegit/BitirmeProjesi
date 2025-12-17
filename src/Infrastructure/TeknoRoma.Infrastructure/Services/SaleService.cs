using TeknoRoma.Application.DTOs.Sale;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Application.Interfaces.Services;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Services;

public class SaleService : ISaleService
{
    private readonly IUnitOfWork _unitOfWork;

    public SaleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<SaleDto>> GetAllSalesAsync()
    {
        var sales = await _unitOfWork.Sales.GetAllAsync();
        return sales.Select(MapToDto);
    }

    public async Task<IEnumerable<SaleDto>> GetSalesWithDetailsAsync()
    {
        var sales = await _unitOfWork.Sales.GetSalesWithDetailsAsync();
        return sales.Select(MapToDtoWithDetails);
    }

    public async Task<SaleDto?> GetSaleByIdAsync(int id)
    {
        var sale = await _unitOfWork.Sales.GetByIdAsync(id);
        return sale == null ? null : MapToDto(sale);
    }

    public async Task<SaleDto?> GetSaleWithDetailsAsync(int id)
    {
        var sale = await _unitOfWork.Sales.GetSaleWithDetailsAsync(id);
        return sale == null ? null : MapToDtoWithDetails(sale);
    }

    public async Task<IEnumerable<SaleDto>> GetSalesByCustomerIdAsync(int customerId)
    {
        var sales = await _unitOfWork.Sales.GetSalesByCustomerIdAsync(customerId);
        return sales.Select(MapToDtoWithDetails);
    }

    public async Task<IEnumerable<SaleDto>> GetSalesByEmployeeIdAsync(int employeeId)
    {
        var sales = await _unitOfWork.Sales.GetSalesByEmployeeIdAsync(employeeId);
        return sales.Select(MapToDtoWithDetails);
    }

    public async Task<IEnumerable<SaleDto>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var sales = await _unitOfWork.Sales.GetSalesByDateRangeAsync(startDate, endDate);
        return sales.Select(MapToDtoWithDetails);
    }

    public async Task<IEnumerable<SaleDto>> GetSalesByStatusAsync(string status)
    {
        var sales = await _unitOfWork.Sales.GetSalesByStatusAsync(status);
        return sales.Select(MapToDtoWithDetails);
    }

    public async Task<SaleDto> CreateSaleAsync(CreateSaleDto createSaleDto)
    {
        // Get employee for commission rate
        var employee = await _unitOfWork.Employees.GetByIdAsync(createSaleDto.EmployeeId);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {createSaleDto.EmployeeId} not found.");
        }

        // Verify customer exists
        var customer = await _unitOfWork.Customers.GetByIdAsync(createSaleDto.CustomerId);
        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {createSaleDto.CustomerId} not found.");
        }

        // Generate invoice number
        var invoiceNumber = $"INV-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper()}";

        // Calculate sale totals
        decimal totalAmount = 0;
        var saleDetails = new List<SaleDetail>();

        foreach (var detailDto in createSaleDto.SaleDetails)
        {
            // Verify product exists
            var product = await _unitOfWork.Products.GetByIdAsync(detailDto.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {detailDto.ProductId} not found.");
            }

            // Calculate line totals
            var lineTotal = detailDto.Quantity * detailDto.UnitPrice;
            var lineDiscountAmount = lineTotal * (detailDto.DiscountRate / 100);
            var lineNetPrice = lineTotal - lineDiscountAmount;

            var saleDetail = new SaleDetail
            {
                ProductId = detailDto.ProductId,
                Quantity = detailDto.Quantity,
                UnitPrice = detailDto.UnitPrice,
                DiscountRate = detailDto.DiscountRate,
                DiscountAmount = lineDiscountAmount,
                TotalPrice = lineTotal,
                NetPrice = lineNetPrice
            };

            saleDetails.Add(saleDetail);
            totalAmount += lineTotal;
        }

        // Calculate sale totals
        var netAmount = totalAmount - createSaleDto.DiscountAmount;
        var commissionAmount = netAmount * employee.CommissionRate;

        var sale = new Sale
        {
            SaleDate = createSaleDto.SaleDate,
            CustomerId = createSaleDto.CustomerId,
            EmployeeId = createSaleDto.EmployeeId,
            TotalAmount = totalAmount,
            DiscountAmount = createSaleDto.DiscountAmount,
            NetAmount = netAmount,
            CommissionAmount = commissionAmount,
            PaymentMethod = createSaleDto.PaymentMethod,
            Notes = createSaleDto.Notes,
            Status = "Completed",
            InvoiceNumber = invoiceNumber,
            SaleDetails = saleDetails
        };

        await _unitOfWork.Sales.AddAsync(sale);
        await _unitOfWork.SaveChangesAsync();

        // Reload with details
        var createdSale = await _unitOfWork.Sales.GetSaleWithDetailsAsync(sale.Id);
        return MapToDtoWithDetails(createdSale!);
    }

    public async Task<SaleDto> UpdateSaleAsync(UpdateSaleDto updateSaleDto)
    {
        var sale = await _unitOfWork.Sales.GetByIdAsync(updateSaleDto.Id);

        if (sale == null)
        {
            throw new KeyNotFoundException($"Sale with ID {updateSaleDto.Id} not found.");
        }

        sale.Status = updateSaleDto.Status;
        sale.Notes = updateSaleDto.Notes;

        await _unitOfWork.Sales.UpdateAsync(sale);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(sale);
    }

    public async Task<bool> DeleteSaleAsync(int id)
    {
        var sale = await _unitOfWork.Sales.GetByIdAsync(id);

        if (sale == null)
        {
            return false;
        }

        await _unitOfWork.Sales.DeleteAsync(sale);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<decimal> GetTotalSalesAmountByEmployeeAsync(int employeeId, DateTime startDate, DateTime endDate)
    {
        return await _unitOfWork.Sales.GetTotalSalesAmountByEmployeeAsync(employeeId, startDate, endDate);
    }

    public async Task<decimal> GetTotalCommissionByEmployeeAsync(int employeeId, DateTime startDate, DateTime endDate)
    {
        return await _unitOfWork.Sales.GetTotalCommissionByEmployeeAsync(employeeId, startDate, endDate);
    }

    private static SaleDto MapToDto(Sale sale)
    {
        return new SaleDto
        {
            Id = sale.Id,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            CustomerName = sale.Customer != null ? $"{sale.Customer.FirstName} {sale.Customer.LastName}" : string.Empty,
            EmployeeId = sale.EmployeeId,
            EmployeeName = sale.Employee != null ? $"{sale.Employee.FirstName} {sale.Employee.LastName}" : string.Empty,
            TotalAmount = sale.TotalAmount,
            DiscountAmount = sale.DiscountAmount,
            NetAmount = sale.NetAmount,
            CommissionAmount = sale.CommissionAmount,
            PaymentMethod = sale.PaymentMethod,
            Notes = sale.Notes,
            Status = sale.Status,
            InvoiceNumber = sale.InvoiceNumber,
            CreatedDate = sale.CreatedDate,
            UpdatedDate = sale.UpdatedDate
        };
    }

    private static SaleDto MapToDtoWithDetails(Sale sale)
    {
        var dto = MapToDto(sale);

        if (sale.SaleDetails != null)
        {
            dto.SaleDetails = sale.SaleDetails.Select(sd => new SaleDetailDto
            {
                Id = sd.Id,
                SaleId = sd.SaleId,
                ProductId = sd.ProductId,
                ProductName = sd.Product?.Name ?? string.Empty,
                Quantity = sd.Quantity,
                UnitPrice = sd.UnitPrice,
                DiscountRate = sd.DiscountRate,
                DiscountAmount = sd.DiscountAmount,
                TotalPrice = sd.TotalPrice,
                NetPrice = sd.NetPrice
            }).ToList();
        }

        return dto;
    }
}
