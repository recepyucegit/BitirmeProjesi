using FluentValidation;
using TeknoRoma.Application.DTOs.Sale;

namespace TeknoRoma.Application.Validators;

public class CreateSaleDtoValidator : AbstractValidator<CreateSaleDto>
{
    public CreateSaleDtoValidator()
    {
        RuleFor(x => x.SaleDate)
            .NotEmpty().WithMessage("Sale date is required")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Sale date cannot be in the future");

        RuleFor(x => x.CustomerId)
            .GreaterThan(0).WithMessage("Customer is required");

        RuleFor(x => x.EmployeeId)
            .GreaterThan(0).WithMessage("Employee is required");

        RuleFor(x => x.DiscountAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Discount amount cannot be negative");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty().WithMessage("Payment method is required")
            .Must(x => x == "Cash" || x == "CreditCard" || x == "BankTransfer")
            .WithMessage("Payment method must be Cash, CreditCard, or BankTransfer");

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters");

        RuleFor(x => x.SaleDetails)
            .NotEmpty().WithMessage("Sale details are required")
            .Must(x => x.Count > 0).WithMessage("At least one product is required");

        RuleForEach(x => x.SaleDetails).ChildRules(detail =>
        {
            detail.RuleFor(d => d.ProductId)
                .GreaterThan(0).WithMessage("Product is required");

            detail.RuleFor(d => d.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");

            detail.RuleFor(d => d.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than 0");

            detail.RuleFor(d => d.DiscountRate)
                .InclusiveBetween(0, 100).WithMessage("Discount rate must be between 0 and 100");
        });
    }
}
