using System.ComponentModel.DataAnnotations;

namespace TeknoRoma.Application.DTOs.Sale;

public class UpdateSaleDto
{
    [Required(ErrorMessage = "Id zorunludur")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Durum zorunludur")]
    [RegularExpression("^(Completed|Cancelled|Refunded)$", ErrorMessage = "Durum Completed, Cancelled veya Refunded olmalıdır")]
    public string Status { get; set; } = "Completed";

    [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir")]
    public string? Notes { get; set; }
}
