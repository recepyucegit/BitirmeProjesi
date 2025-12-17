using System.ComponentModel.DataAnnotations;

namespace TeknoRoma.Application.DTOs.SupplierTransaction;

public class UpdateSupplierTransactionDto
{
    [Required(ErrorMessage = "Id zorunludur")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Durum zorunludur")]
    [RegularExpression("^(Ordered|Delivered|Cancelled)$", ErrorMessage = "Durum Ordered, Delivered veya Cancelled olmalıdır")]
    public string Status { get; set; } = "Ordered";

    public DateTime? DeliveryDate { get; set; }

    [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir")]
    public string? Notes { get; set; }
}
