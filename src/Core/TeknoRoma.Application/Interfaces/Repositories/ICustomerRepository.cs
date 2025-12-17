using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Application.Interfaces.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<IEnumerable<Customer>> GetActiveCustomersAsync();
    Task<Customer?> GetCustomerByIdentityNumberAsync(string identityNumber);
    Task<IEnumerable<Customer>> GetCustomersByTypeAsync(string customerType);
    Task<IEnumerable<Customer>> GetCustomersByCityAsync(string city);
    Task<bool> IdentityNumberExistsAsync(string identityNumber);
}
