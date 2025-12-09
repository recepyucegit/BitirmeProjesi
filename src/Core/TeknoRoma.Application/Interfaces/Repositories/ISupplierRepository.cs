using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Application.Interfaces.Repositories;

public interface ISupplierRepository : IRepository<Supplier>
{
    Task<IEnumerable<Supplier>> GetActiveSuppliersAsync();
    Task<Supplier?> GetSupplierByTaxNumberAsync(string taxNumber);
    Task<Supplier?> GetSupplierWithProductsAsync(int id);
}
