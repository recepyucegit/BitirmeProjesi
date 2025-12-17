using System.ComponentModel.DataAnnotations;

namespace TeknoRoma.Application.DTOs.Customer;

public class UpdateCustomerDto
{
    [Required(ErrorMessage = "Id alanı zorunludur")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Ad alanı zorunludur")]
    [StringLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Soyad alanı zorunludur")]
    [StringLength(100, ErrorMessage = "Soyad en fazla 100 karakter olabilir")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "TC Kimlik Numarası zorunludur")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik Numarası 11 karakter olmalıdır")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "TC Kimlik Numarası sadece rakamlardan oluşmalıdır")]
    public string IdentityNumber { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
    [StringLength(100, ErrorMessage = "E-posta en fazla 100 karakter olabilir")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
    [StringLength(20, ErrorMessage = "Telefon en fazla 20 karakter olabilir")]
    public string? Phone { get; set; }

    [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir")]
    public string? Address { get; set; }

    [StringLength(100, ErrorMessage = "Şehir en fazla 100 karakter olabilir")]
    public string? City { get; set; }

    [StringLength(10, ErrorMessage = "Posta kodu en fazla 10 karakter olabilir")]
    public string? PostalCode { get; set; }

    [Required(ErrorMessage = "Müşteri tipi zorunludur")]
    [RegularExpression("^(Individual|Corporate)$", ErrorMessage = "Müşteri tipi Individual veya Corporate olmalıdır")]
    public string CustomerType { get; set; } = "Individual";

    public bool IsActive { get; set; }
}
