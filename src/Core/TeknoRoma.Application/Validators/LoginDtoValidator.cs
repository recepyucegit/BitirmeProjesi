using FluentValidation;
using TeknoRoma.Application.DTOs.User;

namespace TeknoRoma.Application.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Kullanıcı adı boş olamaz");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre boş olamaz");
    }
}
