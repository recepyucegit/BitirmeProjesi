using FluentValidation;
using TeknoRoma.Application.DTOs.User;

namespace TeknoRoma.Application.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Kullanıcı adı boş olamaz")
            .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır")
            .MaximumLength(50).WithMessage("Kullanıcı adı en fazla 50 karakter olabilir")
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Kullanıcı adı sadece harf, rakam ve alt çizgi içerebilir");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta adresi boş olamaz")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz")
            .MaximumLength(100).WithMessage("E-posta adresi en fazla 100 karakter olabilir");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre boş olamaz")
            .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır")
            .MaximumLength(100).WithMessage("Şifre en fazla 100 karakter olabilir");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^(\+90|0)?[0-9]{10}$").WithMessage("Geçerli bir telefon numarası giriniz (örn: 05551234567)")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        RuleFor(x => x.RoleIds)
            .NotEmpty().WithMessage("En az bir rol seçilmelidir");
    }
}
