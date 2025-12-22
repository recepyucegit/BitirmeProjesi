using FluentValidation;
using TeknoRoma.Application.DTOs.Customer;

namespace TeknoRoma.Application.Validators;

public class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
{
    public CreateCustomerDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

        RuleFor(x => x.IdentityNumber)
            .NotEmpty().WithMessage("Identity number is required")
            .Length(11).WithMessage("Identity number must be 11 characters")
            .Matches(@"^\d{11}$").WithMessage("Identity number must contain only digits");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email address")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("Phone cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Address)
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters");

        RuleFor(x => x.City)
            .MaximumLength(100).WithMessage("City cannot exceed 100 characters");

        RuleFor(x => x.PostalCode)
            .MaximumLength(10).WithMessage("Postal code cannot exceed 10 characters");

        RuleFor(x => x.CustomerType)
            .NotEmpty().WithMessage("Customer type is required")
            .Must(x => x == "Individual" || x == "Corporate")
            .WithMessage("Customer type must be Individual or Corporate");
    }
}
