using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeknoRoma.Application.DTOs.Store;
using TeknoRoma.Application.Interfaces.Services;

namespace TeknoRoma.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StoreController : ControllerBase
{
    private readonly IStoreService _storeService;

    public StoreController(IStoreService storeService)
    {
        _storeService = storeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StoreDto>>> GetAllStores()
    {
        var stores = await _storeService.GetAllAsync();
        return Ok(stores);
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<StoreDto>>> GetActiveStores()
    {
        var stores = await _storeService.GetActiveStoresAsync();
        return Ok(stores);
    }

    [HttpGet("city/{city}")]
    public async Task<ActionResult<IEnumerable<StoreDto>>> GetStoresByCity(string city)
    {
        var stores = await _storeService.GetStoresByCityAsync(city);
        return Ok(stores);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StoreDto>> GetStore(int id)
    {
        var store = await _storeService.GetByIdAsync(id);
        if (store == null) return NotFound();
        return Ok(store);
    }

    [HttpGet("code/{storeCode}")]
    public async Task<ActionResult<StoreDto>> GetStoreByCode(string storeCode)
    {
        var store = await _storeService.GetByStoreCodeAsync(storeCode);
        if (store == null) return NotFound();
        return Ok(store);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,StoreManager")]
    public async Task<ActionResult<StoreDto>> CreateStore(CreateStoreDto dto)
    {
        var store = await _storeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetStore), new { id = store.Id }, store);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,StoreManager")]
    public async Task<ActionResult<StoreDto>> UpdateStore(int id, UpdateStoreDto dto)
    {
        var store = await _storeService.UpdateAsync(id, dto);
        return Ok(store);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteStore(int id)
    {
        await _storeService.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("code/{storeCode}/exists")]
    public async Task<ActionResult<bool>> StoreCodeExists(string storeCode)
    {
        var exists = await _storeService.StoreCodeExistsAsync(storeCode);
        return Ok(exists);
    }
}
