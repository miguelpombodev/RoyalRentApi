using FluentValidation;
using RoyalRent.Application.DTOs.Inputs;

namespace RoyalRent.Application.Behaviors.Validators;

public class LoginAccountRequestValidator : AbstractValidator<LoginAccountRequest>
{
    public LoginAccountRequestValidator()
    {
        RuleFor(x => x.Email).EmailAddress().WithMessage("Email address field not valid").NotEmpty()
            .WithMessage("Email address is required");
        RuleFor(x => x.Password).Must(x => x.Length >= 8)
            .WithMessage("Password field must have or be greater than 8 characters").NotEmpty()
            .WithMessage("Password field is required");
    }
}
