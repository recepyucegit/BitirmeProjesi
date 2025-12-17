using TeknoRoma.Application.DTOs.Customer;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Application.Interfaces.Services;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        var customers = await _unitOfWork.Customers.GetAllAsync();
        return customers.Select(MapToDto);
    }

    public async Task<IEnumerable<CustomerDto>> GetActiveCustomersAsync()
    {
        var customers = await _unitOfWork.Customers.GetActiveCustomersAsync();
        return customers.Select(MapToDto);
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);
        return customer == null ? null : MapToDto(customer);
    }

    public async Task<CustomerDto?> GetCustomerByIdentityNumberAsync(string identityNumber)
    {
        var customer = await _unitOfWork.Customers.GetCustomerByIdentityNumberAsync(identityNumber);
        return customer == null ? null : MapToDto(customer);
    }

    public async Task<IEnumerable<CustomerDto>> GetCustomersByTypeAsync(string customerType)
    {
        var customers = await _unitOfWork.Customers.GetCustomersByTypeAsync(customerType);
        return customers.Select(MapToDto);
    }

    public async Task<IEnumerable<CustomerDto>> GetCustomersByCityAsync(string city)
    {
        var customers = await _unitOfWork.Customers.GetCustomersByCityAsync(city);
        return customers.Select(MapToDto);
    }

    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
    {
        var customer = new Customer
        {
            FirstName = createCustomerDto.FirstName,
            LastName = createCustomerDto.LastName,
            IdentityNumber = createCustomerDto.IdentityNumber,
            Email = createCustomerDto.Email,
            Phone = createCustomerDto.Phone,
            Address = createCustomerDto.Address,
            City = createCustomerDto.City,
            PostalCode = createCustomerDto.PostalCode,
            CustomerType = createCustomerDto.CustomerType,
            IsActive = createCustomerDto.IsActive
        };

        await _unitOfWork.Customers.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(customer);
    }

    public async Task<CustomerDto> UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(updateCustomerDto.Id);

        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {updateCustomerDto.Id} not found.");
        }

        customer.FirstName = updateCustomerDto.FirstName;
        customer.LastName = updateCustomerDto.LastName;
        customer.IdentityNumber = updateCustomerDto.IdentityNumber;
        customer.Email = updateCustomerDto.Email;
        customer.Phone = updateCustomerDto.Phone;
        customer.Address = updateCustomerDto.Address;
        customer.City = updateCustomerDto.City;
        customer.PostalCode = updateCustomerDto.PostalCode;
        customer.CustomerType = updateCustomerDto.CustomerType;
        customer.IsActive = updateCustomerDto.IsActive;

        await _unitOfWork.Customers.UpdateAsync(customer);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(customer);
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);

        if (customer == null)
        {
            return false;
        }

        await _unitOfWork.Customers.DeleteAsync(customer);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> IdentityNumberExistsAsync(string identityNumber)
    {
        return await _unitOfWork.Customers.IdentityNumberExistsAsync(identityNumber);
    }

    public async Task<bool> IdentityNumberExistsForOtherCustomerAsync(string identityNumber, int customerId)
    {
        var customer = await _unitOfWork.Customers.GetCustomerByIdentityNumberAsync(identityNumber);
        return customer != null && customer.Id != customerId;
    }

    private static CustomerDto MapToDto(Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            IdentityNumber = customer.IdentityNumber,
            Email = customer.Email,
            Phone = customer.Phone,
            Address = customer.Address,
            City = customer.City,
            PostalCode = customer.PostalCode,
            CustomerType = customer.CustomerType,
            IsActive = customer.IsActive,
            CreatedDate = customer.CreatedDate,
            UpdatedDate = customer.UpdatedDate
        };
    }
}
