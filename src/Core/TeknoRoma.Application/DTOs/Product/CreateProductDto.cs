namespace TeknoRoma.Application.DTOs.Product;

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public decimal Price { get; set; }
    public decimal CostPrice { get; set; }
    public int StockQuantity { get; set; }
    public int CriticalStockLevel { get; set; } = 10;
    public bool IsActive { get; set; } = true;
    public string? ImageUrl { get; set; }
    public int CategoryId { get; set; }
}
