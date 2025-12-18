using TeknoRoma.Application.DTOs.Store;
using TeknoRoma.Application.Interfaces.Repositories;
using TeknoRoma.Application.Interfaces.Services;
using TeknoRoma.Domain.Entities;

namespace TeknoRoma.Infrastructure.Services;

public class StoreService : IStoreService
{
    private readonly IUnitOfWork _unitOfWork;

    public StoreService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<StoreDto?> GetByIdAsync(int id)
    {
        var store = await _unitOfWork.Stores.GetByIdWithDetailsAsync(id);
        if (store == null) return null;

        return await MapToDtoAsync(store);
    }

    public async Task<StoreDto?> GetByStoreCodeAsync(string storeCode)
    {
        var store = await _unitOfWork.Stores.GetByStoreCodeAsync(storeCode);
        if (store == null) return null;

        return await MapToDtoAsync(store);
    }

    public async Task<IEnumerable<StoreDto>> GetAllAsync()
    {
        var stores = await _unitOfWork.Stores.GetAllWithDetailsAsync();
        var storeDtos = new List<StoreDto>();

        foreach (var store in stores)
        {
            storeDtos.Add(await MapToDtoAsync(store));
        }

        return storeDtos;
    }

    public async Task<IEnumerable<StoreDto>> GetActiveStoresAsync()
    {
        var stores = await _unitOfWork.Stores.GetActiveStoresAsync();
        var storeDtos = new List<StoreDto>();

        foreach (var store in stores)
        {
            storeDtos.Add(await MapToDtoAsync(store));
        }

        return storeDtos;
    }

    public async Task<IEnumerable<StoreDto>> GetStoresByCityAsync(string city)
    {
        var stores = await _unitOfWork.Stores.GetStoresByCityAsync(city);
        var storeDtos = new List<StoreDto>();

        foreach (var store in stores)
        {
            storeDtos.Add(await MapToDtoAsync(store));
        }

        return storeDtos;
    }

    public async Task<StoreDto> CreateAsync(CreateStoreDto dto)
    {
        // Validate store code uniqueness
        if (await _unitOfWork.Stores.StoreCodeExistsAsync(dto.StoreCode))
            throw new InvalidOperationException("Store code already exists");

        // Validate manager if provided
        if (dto.ManagerId.HasValue)
        {
            var manager = await _unitOfWork.Employees.GetByIdAsync(dto.ManagerId.Value);
            if (manager == null)
                throw new InvalidOperationException("Manager not found");
        }

        var store = new Store
        {
            StoreName = dto.StoreName,
            StoreCode = dto.StoreCode,
            Address = dto.Address,
            City = dto.City,
            District = dto.District,
            Phone = dto.Phone,
            Email = dto.Email,
            ManagerId = dto.ManagerId,
            OpeningDate = dto.OpeningDate,
            MonthlyTarget = dto.MonthlyTarget,
            Capacity = dto.Capacity,
            IsActive = true,
            CreatedDate = DateTime.Now
        };

        await _unitOfWork.Stores.AddAsync(store);
        await _unitOfWork.SaveChangesAsync();

        var createdStore = await _unitOfWork.Stores.GetByIdWithDetailsAsync(store.Id);
        return await MapToDtoAsync(createdStore!);
    }

    public async Task<StoreDto> UpdateAsync(int id, UpdateStoreDto dto)
    {
        var store = await _unitOfWork.Stores.GetByIdAsync(id);
        if (store == null)
            throw new InvalidOperationException("Store not found");

        // Check store code uniqueness (excluding current store)
        var existingStore = await _unitOfWork.Stores.GetByStoreCodeAsync(dto.StoreCode);
        if (existingStore != null && existingStore.Id != id)
            throw new InvalidOperationException("Store code already exists");

        // Validate manager if provided
        if (dto.ManagerId.HasValue)
        {
            var manager = await _unitOfWork.Employees.GetByIdAsync(dto.ManagerId.Value);
            if (manager == null)
                throw new InvalidOperationException("Manager not found");
        }

        store.StoreName = dto.StoreName;
        store.StoreCode = dto.StoreCode;
        store.Address = dto.Address;
        store.City = dto.City;
        store.District = dto.District;
        store.Phone = dto.Phone;
        store.Email = dto.Email;
        store.ManagerId = dto.ManagerId;
        store.IsActive = dto.IsActive;
        store.OpeningDate = dto.OpeningDate;
        store.MonthlyTarget = dto.MonthlyTarget;
        store.Capacity = dto.Capacity;
        store.UpdatedDate = DateTime.Now;

        await _unitOfWork.Stores.UpdateAsync(store);
        await _unitOfWork.SaveChangesAsync();

        var updatedStore = await _unitOfWork.Stores.GetByIdWithDetailsAsync(id);
        return await MapToDtoAsync(updatedStore!);
    }

    public async Task DeleteAsync(int id)
    {
        var store = await _unitOfWork.Stores.GetByIdAsync(id);
        if (store == null)
            throw new InvalidOperationException("Store not found");

        await _unitOfWork.Stores.DeleteAsync(store);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<bool> StoreCodeExistsAsync(string storeCode)
    {
        return await _unitOfWork.Stores.StoreCodeExistsAsync(storeCode);
    }

    private async Task<StoreDto> MapToDtoAsync(Store store)
    {
        var employeeCount = await _unitOfWork.Stores.GetEmployeeCountAsync(store.Id);

        return new StoreDto
        {
            Id = store.Id,
            StoreName = store.StoreName,
            StoreCode = store.StoreCode,
            Address = store.Address,
            City = store.City,
            District = store.District,
            Phone = store.Phone,
            Email = store.Email,
            ManagerId = store.ManagerId,
            ManagerName = store.Manager != null ? $"{store.Manager.FirstName} {store.Manager.LastName}" : null,
            IsActive = store.IsActive,
            OpeningDate = store.OpeningDate,
            MonthlyTarget = store.MonthlyTarget,
            Capacity = store.Capacity,
            EmployeeCount = employeeCount,
            CreatedDate = store.CreatedDate
        };
    }
}
