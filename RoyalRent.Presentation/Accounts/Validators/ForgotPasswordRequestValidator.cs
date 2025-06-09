using FluentValidation;
using RoyalRent.Presentation.Accounts.Requests;

namespace RoyalRent.Presentation.Accounts.Validators;

public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(x => x.email).EmailAddress().WithMessage("Email address field not valid").NotEmpty()
            .WithMessage("Email address is required");
        RuleFor(x => x.newPassword).Must(x => x.Length >= 8)
            .WithMessage("Password field must have or be greater than 8 characters").NotEmpty()
            .WithMessage("Password field is required");
    }
}
