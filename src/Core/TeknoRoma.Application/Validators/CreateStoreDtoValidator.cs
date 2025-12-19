using FluentValidation;
using TeknoRoma.Application.DTOs.Store;

namespace TeknoRoma.Application.Validators;

public class CreateStoreDtoValidator : AbstractValidator<CreateStoreDto>
{
    public CreateStoreDtoValidator()
    {
        RuleFor(x => x.StoreName)
            .NotEmpty().WithMessage("Mağaza adı boş olamaz")
            .MaximumLength(100).WithMessage("Mağaza adı en fazla 100 karakter olabilir");

        RuleFor(x => x.StoreCode)
            .NotEmpty().WithMessage("Mağaza kodu boş olamaz")
            .MaximumLength(20).WithMessage("Mağaza kodu en fazla 20 karakter olabilir")
            .Matches("^[A-Z0-9]+$").WithMessage("Mağaza kodu sadece büyük harf ve rakam içerebilir");

        RuleFor(x => x.City)
            .MaximumLength(50).WithMessage("Şehir adı en fazla 50 karakter olabilir")
            .When(x => !string.IsNullOrEmpty(x.City));

        RuleFor(x => x.District)
            .MaximumLength(50).WithMessage("İlçe adı en fazla 50 karakter olabilir")
            .When(x => !string.IsNullOrEmpty(x.District));

        RuleFor(x => x.Phone)
            .Matches(@"^(\+90|0)?[0-9]{10}$").WithMessage("Geçerli bir telefon numarası giriniz")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.MonthlyTarget)
            .GreaterThanOrEqualTo(0).WithMessage("Aylık hedef negatif olamaz")
            .When(x => x.MonthlyTarget.HasValue);

        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("Kapasite sıfırdan büyük olmalıdır")
            .When(x => x.Capacity.HasValue);

        RuleFor(x => x.OpeningDate)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Açılış tarihi gelecek tarih olamaz");
    }
}
