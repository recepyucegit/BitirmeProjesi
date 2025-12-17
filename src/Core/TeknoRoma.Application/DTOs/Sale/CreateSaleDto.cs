using System.ComponentModel.DataAnnotations;

namespace TeknoRoma.Application.DTOs.Sale;

public class CreateSaleDto
{
    public DateTime SaleDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Müşteri ID zorunludur")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Çalışan ID zorunludur")]
    public int EmployeeId { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "İndirim tutarı 0'dan küçük olamaz")]
    public decimal DiscountAmount { get; set; } = 0;

    [Required(ErrorMessage = "Ödeme yöntemi zorunludur")]
    [RegularExpression("^(Cash|CreditCard|BankTransfer)$", ErrorMessage = "Ödeme yöntemi Cash, CreditCard veya BankTransfer olmalıdır")]
    public string PaymentMethod { get; set; } = "Cash";

    [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir")]
    public string? Notes { get; set; }

    [Required(ErrorMessage = "Satış detayları zorunludur")]
    [MinLength(1, ErrorMessage = "En az 1 ürün olmalıdır")]
    public List<CreateSaleDetailDto> SaleDetails { get; set; } = new();
}
