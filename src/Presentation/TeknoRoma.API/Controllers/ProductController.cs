using Microsoft.AspNetCore.Mvc;
using TeknoRoma.Application.DTOs.Product;
using TeknoRoma.Application.Interfaces.Services;

namespace TeknoRoma.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetActiveProducts()
    {
        var products = await _productService.GetActiveProductsAsync();
        return Ok(products);
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetLowStockProducts()
    {
        var products = await _productService.GetLowStockProductsAsync();
        return Ok(products);
    }

    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategory(int categoryId)
    {
        var products = await _productService.GetProductsByCategoryAsync(categoryId);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpGet("barcode/{barcode}")]
    public async Task<ActionResult<ProductDto>> GetProductByBarcode(string barcode)
    {
        var product = await _productService.GetProductByBarcodeAsync(barcode);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto dto)
    {
        var product = await _productService.CreateProductAsync(dto);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDto>> UpdateProduct(int id, UpdateProductDto dto)
    {
        if (id != dto.Id) return BadRequest();
        var product = await _productService.UpdateProductAsync(dto);
        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _productService.DeleteProductAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpGet("{id}/exists")]
    public async Task<ActionResult<bool>> ProductExists(int id)
    {
        var exists = await _productService.ProductExistsAsync(id);
        return Ok(exists);
    }

    [HttpGet("barcode/{barcode}/exists")]
    public async Task<ActionResult<bool>> BarcodeExists(string barcode)
    {
        var exists = await _productService.BarcodeExistsAsync(barcode);
        return Ok(exists);
    }
}
