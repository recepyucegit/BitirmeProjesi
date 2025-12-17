using System.ComponentModel.DataAnnotations;

namespace TeknoRoma.Application.DTOs.SupplierTransaction;

public class CreateSupplierTransactionDetailDto
{
    [Required(ErrorMessage = "Ürün ID zorunludur")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Miktar zorunludur")]
    [Range(1, int.MaxValue, ErrorMessage = "Miktar en az 1 olmalıdır")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Birim fiyat zorunludur")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Birim fiyat 0'dan büyük olmalıdır")]
    public decimal UnitPrice { get; set; }
}
