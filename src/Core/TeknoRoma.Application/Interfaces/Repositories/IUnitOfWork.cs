namespace TeknoRoma.Application.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository Categories { get; }
    IProductRepository Products { get; }
    ISupplierRepository Suppliers { get; }
    IEmployeeRepository Employees { get; }
    ICustomerRepository Customers { get; }
    ISaleRepository Sales { get; }
    ISupplierTransactionRepository SupplierTransactions { get; }
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    IUserRoleRepository UserRoles { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
