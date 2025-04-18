using FluentValidation;
using RoyalRent.Presentation.Accounts.Requests;

namespace RoyalRent.Presentation.Accounts.Validators;

public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
{
    public CreateAccountRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("User name is required");
        RuleFor(x => x.Cpf).NotEmpty().WithMessage("Cpf number is required");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email address is required");
        RuleFor(x => x.Telephone).NotEmpty().WithMessage("Telephone number is required");

        RuleFor(x => x.Email).EmailAddress().WithMessage("Email Address must be a valid one");

    }
}
