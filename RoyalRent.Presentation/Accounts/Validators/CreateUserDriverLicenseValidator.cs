using System.Data;
using FluentValidation;
using RoyalRent.Application.DTOs;

namespace RoyalRent.Presentation.Accounts.Validators;

public class CreateUserDriverLicenseValidator : AbstractValidator<CreateUserDriverLicenseDto>
{
    private const int RgLength = 9;

    public CreateUserDriverLicenseValidator()
    {
        RuleFor(x => x.Rg).Length(RgLength).NotEmpty().WithMessage("Field RG must be required");
        RuleFor(x => x.BirthDate).NotEmpty().WithMessage("Field birth date must be required");
        RuleFor(x => x.DriverLicenseNumber).NotEmpty().WithMessage("Field CNH number must be required");
        RuleFor(x => x.DocumentExpirationDate).NotEmpty().WithMessage("Field Document Expiration Date must be required")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("CNH Expiration date must be less or equal than current date");
        RuleFor(x => x.State).NotEmpty().WithMessage("Field State must be required");
    }
}
