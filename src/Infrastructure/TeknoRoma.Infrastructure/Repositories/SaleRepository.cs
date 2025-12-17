using Microsoft.EntityFrameworkCore;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Domain.Entities;
using TeknoRoma.Infrastructure.Data;

namespace TeknoRoma.Infrastructure.Repositories;

public class SaleRepository : Repository<Sale>, ISaleRepository
{
    public SaleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Sale?> GetSaleWithDetailsAsync(int id)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
    }

    public async Task<IEnumerable<Sale>> GetSalesWithDetailsAsync()
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
            .Where(s => !s.IsDeleted)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Sale>> GetSalesByCustomerIdAsync(int customerId)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .Include(s => s.SaleDetails)
            .Where(s => s.CustomerId == customerId && !s.IsDeleted)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Sale>> GetSalesByEmployeeIdAsync(int employeeId)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .Include(s => s.SaleDetails)
            .Where(s => s.EmployeeId == employeeId && !s.IsDeleted)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .Include(s => s.SaleDetails)
            .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate && !s.IsDeleted)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Sale>> GetSalesByStatusAsync(string status)
    {
        return await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .Include(s => s.SaleDetails)
            .Where(s => s.Status == status && !s.IsDeleted)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalSalesAmountByEmployeeAsync(int employeeId, DateTime startDate, DateTime endDate)
    {
        return await _context.Sales
            .Where(s => s.EmployeeId == employeeId
                && s.SaleDate >= startDate
                && s.SaleDate <= endDate
                && s.Status == "Completed"
                && !s.IsDeleted)
            .SumAsync(s => s.NetAmount);
    }

    public async Task<decimal> GetTotalCommissionByEmployeeAsync(int employeeId, DateTime startDate, DateTime endDate)
    {
        return await _context.Sales
            .Where(s => s.EmployeeId == employeeId
                && s.SaleDate >= startDate
                && s.SaleDate <= endDate
                && s.Status == "Completed"
                && !s.IsDeleted)
            .SumAsync(s => s.CommissionAmount);
    }
}
