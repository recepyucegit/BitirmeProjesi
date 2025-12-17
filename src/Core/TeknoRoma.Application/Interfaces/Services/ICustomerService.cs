using TeknoRoma.Application.DTOs.Customer;

namespace TeknoRoma.Application.Interfaces.Services;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
    Task<IEnumerable<CustomerDto>> GetActiveCustomersAsync();
    Task<CustomerDto?> GetCustomerByIdAsync(int id);
    Task<CustomerDto?> GetCustomerByIdentityNumberAsync(string identityNumber);
    Task<IEnumerable<CustomerDto>> GetCustomersByTypeAsync(string customerType);
    Task<IEnumerable<CustomerDto>> GetCustomersByCityAsync(string city);
    Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
    Task<CustomerDto> UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto);
    Task<bool> DeleteCustomerAsync(int id);
    Task<bool> IdentityNumberExistsAsync(string identityNumber);
    Task<bool> IdentityNumberExistsForOtherCustomerAsync(string identityNumber, int customerId);
}
