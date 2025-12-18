using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Application.Interfaces.Repositories;

public interface IStoreRepository : IRepository<Store>
{
    Task<Store?> GetByStoreCodeAsync(string storeCode);
    Task<Store?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Store>> GetAllWithDetailsAsync();
    Task<IEnumerable<Store>> GetActiveStoresAsync();
    Task<IEnumerable<Store>> GetStoresByCityAsync(string city);
    Task<IEnumerable<Store>> GetStoresByManagerAsync(int managerId);
    Task<bool> StoreCodeExistsAsync(string storeCode);
    Task<int> GetEmployeeCountAsync(int storeId);
}
