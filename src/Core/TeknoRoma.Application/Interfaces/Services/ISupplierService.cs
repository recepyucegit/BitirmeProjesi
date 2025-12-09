using TeknoRoma.Application.DTOs.Supplier;

namespace TeknoRoma.Application.Interfaces.Services;

public interface ISupplierService
{
    Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync();
    Task<IEnumerable<SupplierDto>> GetActiveSuppliersAsync();
    Task<SupplierDto?> GetSupplierByIdAsync(int id);
    Task<SupplierDto?> GetSupplierByTaxNumberAsync(string taxNumber);
    Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto dto);
    Task<SupplierDto> UpdateSupplierAsync(UpdateSupplierDto dto);
    Task<bool> DeleteSupplierAsync(int id);
    Task<bool> SupplierExistsAsync(int id);
    Task<bool> TaxNumberExistsAsync(string taxNumber);
}
