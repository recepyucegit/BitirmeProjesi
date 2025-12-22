using FluentValidation;
using TeknoRoma.Application.DTOs.Supplier;

namespace TeknoRoma.Application.Validators;

public class CreateSupplierDtoValidator : AbstractValidator<CreateSupplierDto>
{
    public CreateSupplierDtoValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company name is required")
            .MaximumLength(200).WithMessage("Company name cannot exceed 200 characters");

        RuleFor(x => x.ContactName)
            .MaximumLength(100).WithMessage("Contact name cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.ContactName));

        RuleFor(x => x.ContactTitle)
            .MaximumLength(100).WithMessage("Contact title cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.ContactTitle));

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

        RuleFor(x => x.Country)
            .MaximumLength(100).WithMessage("Country cannot exceed 100 characters");

        RuleFor(x => x.PostalCode)
            .MaximumLength(10).WithMessage("Postal code cannot exceed 10 characters");

        RuleFor(x => x.TaxNumber)
            .MaximumLength(20).WithMessage("Tax number cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.TaxNumber));
    }
}
