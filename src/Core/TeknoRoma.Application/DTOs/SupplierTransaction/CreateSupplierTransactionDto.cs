using System.ComponentModel.DataAnnotations;

namespace TeknoRoma.Application.DTOs.SupplierTransaction;

public class CreateSupplierTransactionDto
{
    [Required(ErrorMessage = "Tedarikçi ID zorunludur")]
    public int SupplierId { get; set; }

    [Required(ErrorMessage = "Çalışan ID zorunludur")]
    public int EmployeeId { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.Now;

    [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir")]
    public string? Notes { get; set; }

    [Required(ErrorMessage = "Sipariş detayları zorunludur")]
    [MinLength(1, ErrorMessage = "En az 1 ürün olmalıdır")]
    public List<CreateSupplierTransactionDetailDto> Details { get; set; } = new();
}
