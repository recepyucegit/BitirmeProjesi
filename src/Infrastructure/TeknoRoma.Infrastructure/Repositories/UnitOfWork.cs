using Microsoft.EntityFrameworkCore.Storage;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Infrastructure.Data;

namespace TeknoRoma.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    public ICategoryRepository Categories { get; }
    public IProductRepository Products { get; }
    public ISupplierRepository Suppliers { get; }
    public IEmployeeRepository Employees { get; }
    public ICustomerRepository Customers { get; }
    public ISaleRepository Sales { get; }
    public ISupplierTransactionRepository SupplierTransactions { get; }
    public IUserRepository Users { get; }
    public IRoleRepository Roles { get; }
    public IUserRoleRepository UserRoles { get; }
    public IStoreRepository Stores { get; }
    public IExpenseRepository Expenses { get; }
    public IDepartmentRepository Departments { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Categories = new CategoryRepository(_context);
        Products = new ProductRepository(_context);
        Suppliers = new SupplierRepository(_context);
        Employees = new EmployeeRepository(_context);
        Customers = new CustomerRepository(_context);
        Sales = new SaleRepository(_context);
        SupplierTransactions = new SupplierTransactionRepository(_context);
        Users = new UserRepository(_context);
        Roles = new RoleRepository(_context);
        UserRoles = new UserRoleRepository(_context);
        Stores = new StoreRepository(_context);
        Expenses = new ExpenseRepository(_context);
        Departments = new DepartmentRepository(_context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
