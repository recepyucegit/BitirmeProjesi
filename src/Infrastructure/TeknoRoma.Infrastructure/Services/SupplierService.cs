using TeknoRoma.Application.DTOs.Supplier;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Application.Interfaces.Services;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Services;

public class SupplierService : ISupplierService
{
    private readonly IUnitOfWork _unitOfWork;

    public SupplierService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    private static SupplierDto MapToDto(Supplier supplier)
    {
        return new SupplierDto
        {
            Id = supplier.Id,
            CompanyName = supplier.CompanyName,
            ContactName = supplier.ContactName,
            ContactTitle = supplier.ContactTitle,
            Email = supplier.Email,
            Phone = supplier.Phone,
            Address = supplier.Address,
            City = supplier.City,
            Country = supplier.Country,
            PostalCode = supplier.PostalCode,
            TaxNumber = supplier.TaxNumber,
            IsActive = supplier.IsActive,
            CreatedDate = supplier.CreatedDate
        };
    }

    public async Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync()
    {
        var suppliers = await _unitOfWork.Suppliers.GetAllAsync();
        return suppliers.Select(MapToDto);
    }

    public async Task<IEnumerable<SupplierDto>> GetActiveSuppliersAsync()
    {
        var suppliers = await _unitOfWork.Suppliers.GetActiveSuppliersAsync();
        return suppliers.Select(MapToDto);
    }

    public async Task<SupplierDto?> GetSupplierByIdAsync(int id)
    {
        var supplier = await _unitOfWork.Suppliers.GetByIdAsync(id);
        return supplier == null ? null : MapToDto(supplier);
    }

    public async Task<SupplierDto?> GetSupplierByTaxNumberAsync(string taxNumber)
    {
        var supplier = await _unitOfWork.Suppliers.GetSupplierByTaxNumberAsync(taxNumber);
        return supplier == null ? null : MapToDto(supplier);
    }

    public async Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto dto)
    {
        var supplier = new Supplier
        {
            CompanyName = dto.CompanyName,
            ContactName = dto.ContactName,
            ContactTitle = dto.ContactTitle,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = dto.Address,
            City = dto.City,
            Country = dto.Country,
            PostalCode = dto.PostalCode,
            TaxNumber = dto.TaxNumber,
            IsActive = dto.IsActive
        };

        await _unitOfWork.Suppliers.AddAsync(supplier);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(supplier);
    }

    public async Task<SupplierDto> UpdateSupplierAsync(UpdateSupplierDto dto)
    {
        var supplier = await _unitOfWork.Suppliers.GetByIdAsync(dto.Id);
        if (supplier == null) throw new Exception("Supplier not found");

        supplier.CompanyName = dto.CompanyName;
        supplier.ContactName = dto.ContactName;
        supplier.ContactTitle = dto.ContactTitle;
        supplier.Email = dto.Email;
        supplier.Phone = dto.Phone;
        supplier.Address = dto.Address;
        supplier.City = dto.City;
        supplier.Country = dto.Country;
        supplier.PostalCode = dto.PostalCode;
        supplier.TaxNumber = dto.TaxNumber;
        supplier.IsActive = dto.IsActive;

        await _unitOfWork.Suppliers.UpdateAsync(supplier);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(supplier);
    }

    public async Task<bool> DeleteSupplierAsync(int id)
    {
        var supplier = await _unitOfWork.Suppliers.GetByIdAsync(id);
        if (supplier == null) return false;

        await _unitOfWork.Suppliers.DeleteAsync(supplier);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SupplierExistsAsync(int id)
    {
        return await _unitOfWork.Suppliers.AnyAsync(s => s.Id == id);
    }

    public async Task<bool> TaxNumberExistsAsync(string taxNumber)
    {
        return await _unitOfWork.Suppliers.AnyAsync(s => s.TaxNumber == taxNumber);
    }
}
