using TeknoRoma.Application.DTOs.Store;

namespace TeknoRoma.Application.Interfaces.Services;

public interface IStoreService
{
    Task<StoreDto?> GetByIdAsync(int id);
    Task<StoreDto?> GetByStoreCodeAsync(string storeCode);
    Task<IEnumerable<StoreDto>> GetAllAsync();
    Task<IEnumerable<StoreDto>> GetActiveStoresAsync();
    Task<IEnumerable<StoreDto>> GetStoresByCityAsync(string city);
    Task<StoreDto> CreateAsync(CreateStoreDto dto);
    Task<StoreDto> UpdateAsync(int id, UpdateStoreDto dto);
    Task DeleteAsync(int id);
    Task<bool> StoreCodeExistsAsync(string storeCode);
}
