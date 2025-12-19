using FluentValidation;
using TeknoRoma.Application.DTOs.Expense;

namespace TeknoRoma.Application.Validators;

public class CreateExpenseDtoValidator : AbstractValidator<CreateExpenseDto>
{
    public CreateExpenseDtoValidator()
    {
        RuleFor(x => x.ExpenseType)
            .NotEmpty().WithMessage("Gider türü boş olamaz")
            .MaximumLength(50).WithMessage("Gider türü en fazla 50 karakter olabilir");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Açıklama boş olamaz")
            .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Tutar sıfırdan büyük olmalıdır");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Para birimi boş olamaz")
            .Must(x => x == "TL" || x == "USD" || x == "EUR" || x == "GBP")
            .WithMessage("Geçerli para birimleri: TL, USD, EUR, GBP");

        RuleFor(x => x.ExchangeRate)
            .GreaterThan(0).WithMessage("Döviz kuru sıfırdan büyük olmalıdır");

        RuleFor(x => x.ExpenseDate)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Gider tarihi gelecek tarih olamaz");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Kategori boş olamaz")
            .MaximumLength(50).WithMessage("Kategori en fazla 50 karakter olabilir");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty().WithMessage("Ödeme yöntemi boş olamaz")
            .MaximumLength(50).WithMessage("Ödeme yöntemi en fazla 50 karakter olabilir");

        RuleFor(x => x.InvoiceNumber)
            .MaximumLength(50).WithMessage("Fatura numarası en fazla 50 karakter olabilir")
            .When(x => !string.IsNullOrEmpty(x.InvoiceNumber));

        RuleFor(x => x.Vendor)
            .MaximumLength(100).WithMessage("Satıcı adı en fazla 100 karakter olabilir")
            .When(x => !string.IsNullOrEmpty(x.Vendor));
    }
}
