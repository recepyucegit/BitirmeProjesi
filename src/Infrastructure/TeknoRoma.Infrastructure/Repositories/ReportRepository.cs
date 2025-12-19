using Microsoft.EntityFrameworkCore;
using TeknoRoma.Application.Common;
using TeknoRoma.Application.DTOs.Report;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Infrastructure.Data;

namespace TeknoRoma.Infrastructure.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly ApplicationDbContext _context;

    public ReportRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    #region Sales Reports

    public async Task<PagedResult<SalesReportDto>> GetSalesReportAsync(
        DateTime? startDate,
        DateTime? endDate,
        int? categoryId,
        int? storeId,
        PaginationParams paginationParams)
    {
        var query = _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .Include(s => s.Store)
            .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                    .ThenInclude(p => p!.Category)
            .Where(s => !s.IsDeleted);

        // Filters
        if (startDate.HasValue)
            query = query.Where(s => s.SaleDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(s => s.SaleDate <= endDate.Value);

        if (storeId.HasValue)
            query = query.Where(s => s.StoreId == storeId.Value);

        if (categoryId.HasValue)
            query = query.Where(s => s.SaleDetails!.Any(sd => sd.Product!.CategoryId == categoryId.Value));

        var totalCount = await query.CountAsync();

        // Sorting
        query = paginationParams.SortBy?.ToLower() switch
        {
            "date" => paginationParams.SortOrder.ToLower() == "desc"
                ? query.OrderByDescending(s => s.SaleDate)
                : query.OrderBy(s => s.SaleDate),
            "total" => paginationParams.SortOrder.ToLower() == "desc"
                ? query.OrderByDescending(s => s.TotalAmount)
                : query.OrderBy(s => s.TotalAmount),
            _ => query.OrderByDescending(s => s.SaleDate)
        };

        // Pagination
        var sales = await query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .Select(s => new SalesReportDto
            {
                SaleId = s.Id,
                SaleDate = s.SaleDate,
                CustomerName = s.Customer!.FirstName + " " + s.Customer.LastName,
                CustomerTC = s.Customer.IdentityNumber,
                EmployeeName = s.Employee != null ? s.Employee.FirstName + " " + s.Employee.LastName : null,
                StoreName = s.Store != null ? s.Store.StoreName : null,
                TotalItems = s.SaleDetails!.Sum(sd => sd.Quantity),
                TotalAmount = s.TotalAmount,
                PaymentMethod = s.PaymentMethod,
                Items = s.SaleDetails!.Select(sd => new SalesReportItemDto
                {
                    ProductName = sd.Product!.Name,
                    CategoryName = sd.Product.Category!.Name,
                    Quantity = sd.Quantity,
                    UnitPrice = sd.UnitPrice,
                    DiscountAmount = sd.DiscountAmount,
                    TotalPrice = sd.TotalPrice
                }).ToList()
            })
            .ToListAsync();

        return PagedResult<SalesReportDto>.Create(sales, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<SalesSummaryDto> GetSalesSummaryAsync(DateTime? startDate, DateTime? endDate)
    {
        var query = _context.Sales
            .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                    .ThenInclude(p => p!.Category)
            .Include(s => s.Store)
            .Where(s => !s.IsDeleted);

        if (startDate.HasValue)
            query = query.Where(s => s.SaleDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(s => s.SaleDate <= endDate.Value);

        var sales = await query.ToListAsync();

        var summary = new SalesSummaryDto
        {
            StartDate = startDate ?? sales.Min(s => s.SaleDate),
            EndDate = endDate ?? sales.Max(s => s.SaleDate),
            TotalSalesCount = sales.Count,
            TotalRevenue = sales.Sum(s => s.TotalAmount),
            AverageSaleAmount = sales.Any() ? sales.Average(s => s.TotalAmount) : 0,
            TotalProductsSold = sales.SelectMany(s => s.SaleDetails!).Sum(sd => sd.Quantity),
            UniqueCustomers = sales.Select(s => s.CustomerId).Distinct().Count(),
            SalesByCategory = sales
                .SelectMany(s => s.SaleDetails!)
                .GroupBy(sd => sd.Product!.Category!.Name)
                .ToDictionary(g => g.Key, g => g.Sum(sd => sd.TotalPrice)),
            SalesByStore = sales
                .Where(s => s.Store != null)
                .GroupBy(s => s.Store!.StoreName)
                .ToDictionary(g => g.Key, g => g.Sum(s => s.TotalAmount)),
            SalesByPaymentMethod = sales
                .GroupBy(s => s.PaymentMethod)
                .ToDictionary(g => g.Key, g => g.Count())
        };

        return summary;
    }

    public async Task<List<TopProductDto>> GetTopSellingProductsAsync(int count, DateTime? startDate, DateTime? endDate)
    {
        var query = _context.SaleDetails
            .Include(sd => sd.Product)
                .ThenInclude(p => p!.Category)
            .Include(sd => sd.Sale)
            .Where(sd => !sd.IsDeleted && !sd.Sale!.IsDeleted);

        if (startDate.HasValue)
            query = query.Where(sd => sd.Sale!.SaleDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(sd => sd.Sale!.SaleDate <= endDate.Value);

        var topProducts = await query
            .GroupBy(sd => new
            {
                sd.ProductId,
                ProductName = sd.Product!.Name,
                CategoryName = sd.Product.Category!.Name
            })
            .Select(g => new TopProductDto
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.ProductName,
                CategoryName = g.Key.CategoryName,
                TotalQuantitySold = g.Sum(sd => sd.Quantity),
                TotalRevenue = g.Sum(sd => sd.TotalPrice)
            })
            .OrderByDescending(p => p.TotalRevenue)
            .Take(count)
            .ToListAsync();

        return topProducts;
    }

    public async Task<List<TopCustomerDto>> GetTopCustomersAsync(int count, DateTime? startDate, DateTime? endDate)
    {
        var query = _context.Sales
            .Include(s => s.Customer)
            .Where(s => !s.IsDeleted);

        if (startDate.HasValue)
            query = query.Where(s => s.SaleDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(s => s.SaleDate <= endDate.Value);

        var topCustomers = await query
            .GroupBy(s => new
            {
                s.CustomerId,
                CustomerName = s.Customer!.FirstName + " " + s.Customer.LastName
            })
            .Select(g => new TopCustomerDto
            {
                CustomerId = g.Key.CustomerId,
                CustomerName = g.Key.CustomerName,
                TotalPurchases = g.Count(),
                TotalSpent = g.Sum(s => s.TotalAmount),
                LastPurchaseDate = g.Max(s => s.SaleDate)
            })
            .OrderByDescending(c => c.TotalSpent)
            .Take(count)
            .ToListAsync();

        return topCustomers;
    }

    public async Task<List<TopEmployeeDto>> GetTopEmployeesAsync(int count, DateTime? startDate, DateTime? endDate)
    {
        var query = _context.Sales
            .Include(s => s.Employee)
                .ThenInclude(e => e!.Store)
            .Where(s => !s.IsDeleted && s.Employee != null);

        if (startDate.HasValue)
            query = query.Where(s => s.SaleDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(s => s.SaleDate <= endDate.Value);

        var topEmployees = await query
            .GroupBy(s => new
            {
                EmployeeId = s.EmployeeId,
                EmployeeName = s.Employee!.FirstName + " " + s.Employee.LastName,
                StoreName = s.Employee.Store != null ? s.Employee.Store.StoreName : null
            })
            .Select(g => new TopEmployeeDto
            {
                EmployeeId = g.Key.EmployeeId,
                EmployeeName = g.Key.EmployeeName,
                StoreName = g.Key.StoreName,
                TotalSales = g.Count(),
                TotalRevenue = g.Sum(s => s.TotalAmount)
            })
            .OrderByDescending(e => e.TotalRevenue)
            .Take(count)
            .ToListAsync();

        return topEmployees;
    }

    #endregion

    #region Stock Reports

    public async Task<PagedResult<StockReportDto>> GetStockReportAsync(
        int? categoryId,
        bool? lowStockOnly,
        PaginationParams paginationParams)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Where(p => !p.IsDeleted && p.IsActive);

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (lowStockOnly == true)
            query = query.Where(p => p.StockQuantity <= p.CriticalStockLevel);

        var totalCount = await query.CountAsync();

        // Sorting
        query = paginationParams.SortBy?.ToLower() switch
        {
            "name" => paginationParams.SortOrder.ToLower() == "desc"
                ? query.OrderByDescending(p => p.Name)
                : query.OrderBy(p => p.Name),
            "stock" => paginationParams.SortOrder.ToLower() == "desc"
                ? query.OrderByDescending(p => p.StockQuantity)
                : query.OrderBy(p => p.StockQuantity),
            "value" => paginationParams.SortOrder.ToLower() == "desc"
                ? query.OrderByDescending(p => p.StockQuantity * p.Price)
                : query.OrderBy(p => p.StockQuantity * p.Price),
            _ => query.OrderBy(p => p.Name)
        };

        var products = await query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .Select(p => new StockReportDto
            {
                ProductId = p.Id,
                ProductName = p.Name,
                CategoryName = p.Category!.Name,
                Barcode = p.Barcode,
                StockQuantity = p.StockQuantity,
                MinimumStockLevel = p.CriticalStockLevel,
                UnitPrice = p.Price,
                SupplierName = p.Supplier != null ? p.Supplier.CompanyName : null,
                LastRestockDate = null
            })
            .ToListAsync();

        return PagedResult<StockReportDto>.Create(products, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<List<LowStockProductDto>> GetLowStockProductsAsync()
    {
        var lowStockProducts = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .Where(p => !p.IsDeleted && p.IsActive && p.StockQuantity <= p.CriticalStockLevel)
            .Select(p => new LowStockProductDto
            {
                ProductId = p.Id,
                ProductName = p.Name,
                CategoryName = p.Category!.Name,
                CurrentStock = p.StockQuantity,
                MinimumStock = p.CriticalStockLevel,
                SupplierName = p.Supplier != null ? p.Supplier.CompanyName : null,
                SupplierPhone = p.Supplier != null ? p.Supplier.Phone : null,
                UnitPrice = p.Price
            })
            .OrderBy(p => p.CurrentStock)
            .ToListAsync();

        return lowStockProducts;
    }

    public async Task<StockSummaryDto> GetStockSummaryAsync()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .Where(p => !p.IsDeleted && p.IsActive)
            .ToListAsync();

        var summary = new StockSummaryDto
        {
            TotalProducts = products.Count,
            LowStockProducts = products.Count(p => p.StockQuantity <= p.CriticalStockLevel && p.StockQuantity > 0),
            OutOfStockProducts = products.Count(p => p.StockQuantity == 0),
            TotalStockValue = products.Sum(p => p.StockQuantity * p.Price),
            StockByCategory = products
                .GroupBy(p => p.Category!.Name)
                .ToDictionary(g => g.Key, g => g.Sum(p => p.StockQuantity)),
            ValueByCategory = products
                .GroupBy(p => p.Category!.Name)
                .ToDictionary(g => g.Key, g => g.Sum(p => p.StockQuantity * p.Price))
        };

        return summary;
    }

    #endregion

    #region Expense Reports

    public async Task<PagedResult<ExpenseReportDto>> GetExpenseReportAsync(
        DateTime? startDate,
        DateTime? endDate,
        int? storeId,
        int? departmentId,
        string? category,
        string? status,
        PaginationParams paginationParams)
    {
        var query = _context.Expenses
            .Include(e => e.Employee)
            .Include(e => e.Store)
            .Include(e => e.Approver)
            .Where(e => !e.IsDeleted);

        if (startDate.HasValue)
            query = query.Where(e => e.ExpenseDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(e => e.ExpenseDate <= endDate.Value);

        if (storeId.HasValue)
            query = query.Where(e => e.StoreId == storeId.Value);

        if (!string.IsNullOrEmpty(category))
            query = query.Where(e => e.Category == category);

        if (!string.IsNullOrEmpty(status))
            query = query.Where(e => e.Status == status);

        var totalCount = await query.CountAsync();

        // Sorting
        query = paginationParams.SortBy?.ToLower() switch
        {
            "date" => paginationParams.SortOrder.ToLower() == "desc"
                ? query.OrderByDescending(e => e.ExpenseDate)
                : query.OrderBy(e => e.ExpenseDate),
            "amount" => paginationParams.SortOrder.ToLower() == "desc"
                ? query.OrderByDescending(e => e.AmountInTL)
                : query.OrderBy(e => e.AmountInTL),
            _ => query.OrderByDescending(e => e.ExpenseDate)
        };

        var expenses = await query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .Select(e => new ExpenseReportDto
            {
                ExpenseId = e.Id,
                ExpenseDate = e.ExpenseDate,
                ExpenseType = e.ExpenseType,
                Category = e.Category,
                Description = e.Description,
                Amount = e.Amount,
                Currency = e.Currency,
                AmountInTL = e.AmountInTL,
                EmployeeName = e.Employee != null ? e.Employee.FirstName + " " + e.Employee.LastName : null,
                StoreName = e.Store != null ? e.Store.StoreName : null,
                Status = e.Status,
                ApproverName = e.Approver != null ? e.Approver.FirstName + " " + e.Approver.LastName : null,
                ApprovalDate = e.ApprovalDate,
                PaymentMethod = e.PaymentMethod,
                Vendor = e.Vendor,
                InvoiceNumber = e.InvoiceNumber
            })
            .ToListAsync();

        return PagedResult<ExpenseReportDto>.Create(expenses, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<ExpenseSummaryDto> GetExpenseSummaryAsync(DateTime? startDate, DateTime? endDate)
    {
        var query = _context.Expenses
            .Include(e => e.Store)
            .Where(e => !e.IsDeleted);

        if (startDate.HasValue)
            query = query.Where(e => e.ExpenseDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(e => e.ExpenseDate <= endDate.Value);

        var expenses = await query.ToListAsync();

        var summary = new ExpenseSummaryDto
        {
            StartDate = startDate ?? (expenses.Any() ? expenses.Min(e => e.ExpenseDate) : DateTime.Now),
            EndDate = endDate ?? (expenses.Any() ? expenses.Max(e => e.ExpenseDate) : DateTime.Now),
            TotalExpenseCount = expenses.Count,
            TotalExpenseAmount = expenses.Sum(e => e.AmountInTL),
            AverageExpenseAmount = expenses.Any() ? expenses.Average(e => e.AmountInTL) : 0,
            PendingExpenseCount = expenses.Count(e => e.Status == "Pending"),
            ApprovedExpenseCount = expenses.Count(e => e.Status == "Approved"),
            RejectedExpenseCount = expenses.Count(e => e.Status == "Rejected"),
            ExpensesByCategory = expenses
                .GroupBy(e => e.Category)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.AmountInTL)),
            ExpensesByStore = expenses
                .Where(e => e.Store != null)
                .GroupBy(e => e.Store!.StoreName)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.AmountInTL)),
            ExpensesByCurrency = expenses
                .GroupBy(e => e.Currency)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount))
        };

        return summary;
    }

    #endregion

    #region Dashboard

    public async Task<DashboardStatsDto> GetDashboardStatsAsync()
    {
        var now = DateTime.Now;
        var today = now.Date;
        var weekStart = today.AddDays(-(int)today.DayOfWeek);
        var monthStart = new DateTime(now.Year, now.Month, 1);
        var yearStart = new DateTime(now.Year, 1, 1);

        // Sales Stats
        var todaySales = await _context.Sales
            .Where(s => !s.IsDeleted && s.SaleDate.Date == today)
            .ToListAsync();

        var weeklySales = await _context.Sales
            .Where(s => !s.IsDeleted && s.SaleDate >= weekStart)
            .ToListAsync();

        var monthlySales = await _context.Sales
            .Where(s => !s.IsDeleted && s.SaleDate >= monthStart)
            .ToListAsync();

        var yearlySales = await _context.Sales
            .Where(s => !s.IsDeleted && s.SaleDate >= yearStart)
            .ToListAsync();

        // Product Stats
        var products = await _context.Products.Where(p => !p.IsDeleted).ToListAsync();

        // Customer Stats
        var customers = await _context.Customers.Where(c => !c.IsDeleted).ToListAsync();
        var newCustomersThisMonth = customers.Count(c => c.CreatedDate >= monthStart);

        // Employee Stats
        var employees = await _context.Employees.Where(e => !e.IsDeleted).ToListAsync();

        // Store Stats
        var stores = await _context.Stores.Where(s => !s.IsDeleted).ToListAsync();

        // Expense Stats
        var monthlyExpenses = await _context.Expenses
            .Where(e => !e.IsDeleted && e.ExpenseDate >= monthStart)
            .SumAsync(e => e.AmountInTL);

        var pendingExpenses = await _context.Expenses
            .CountAsync(e => !e.IsDeleted && e.Status == "Pending");

        // Top Performers
        var topProducts = await GetTopSellingProductsAsync(5, monthStart, null);
        var topCustomers = await GetTopCustomersAsync(5, monthStart, null);
        var topEmployees = await GetTopEmployeesAsync(5, monthStart, null);

        // Recent Activities
        var recentSales = await _context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Employee)
            .Where(s => !s.IsDeleted)
            .OrderByDescending(s => s.SaleDate)
            .Take(5)
            .Select(s => new RecentSaleDto
            {
                SaleId = s.Id,
                SaleDate = s.SaleDate,
                CustomerName = s.Customer!.FirstName + " " + s.Customer.LastName,
                TotalAmount = s.TotalAmount,
                EmployeeName = s.Employee != null ? s.Employee.FirstName + " " + s.Employee.LastName : null
            })
            .ToListAsync();

        var lowStockAlerts = await _context.Products
            .Include(p => p.Supplier)
            .Where(p => !p.IsDeleted && p.IsActive && p.StockQuantity <= p.CriticalStockLevel)
            .OrderBy(p => p.StockQuantity)
            .Take(5)
            .Select(p => new LowStockAlertDto
            {
                ProductId = p.Id,
                ProductName = p.Name,
                CurrentStock = p.StockQuantity,
                MinimumStock = p.CriticalStockLevel,
                SupplierName = p.Supplier != null ? p.Supplier.CompanyName : null
            })
            .ToListAsync();

        return new DashboardStatsDto
        {
            TodaySales = todaySales.Sum(s => s.TotalAmount),
            WeeklySales = weeklySales.Sum(s => s.TotalAmount),
            MonthlySales = monthlySales.Sum(s => s.TotalAmount),
            YearlySales = yearlySales.Sum(s => s.TotalAmount),
            TodaySalesCount = todaySales.Count,
            WeeklySalesCount = weeklySales.Count,
            MonthlySalesCount = monthlySales.Count,

            TotalProducts = products.Count,
            ActiveProducts = products.Count(p => p.IsActive),
            LowStockProducts = products.Count(p => p.IsActive && p.StockQuantity <= p.CriticalStockLevel && p.StockQuantity > 0),
            OutOfStockProducts = products.Count(p => p.IsActive && p.StockQuantity == 0),
            TotalStockValue = products.Sum(p => p.StockQuantity * p.Price),

            TotalCustomers = customers.Count,
            NewCustomersThisMonth = newCustomersThisMonth,
            ActiveCustomers = customers.Count(c => c.IsActive),

            TotalEmployees = employees.Count,
            ActiveEmployees = employees.Count(e => e.IsActive),

            TotalStores = stores.Count,
            ActiveStores = stores.Count(s => s.IsActive),

            MonthlyExpenses = monthlyExpenses,
            PendingExpenses = pendingExpenses,

            TopSellingProducts = topProducts,
            TopCustomers = topCustomers,
            TopEmployees = topEmployees,

            RecentSales = recentSales,
            LowStockAlerts = lowStockAlerts
        };
    }

    #endregion
}
