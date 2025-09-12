using FluentValidation;
using RoyalRent.Application.DTOs.Inputs;
using RoyalRent.Application.Rents.DTOs;

namespace RoyalRent.Application.Behaviors.Validators;

public class CreatePaymentIntentRequestValidator : AbstractValidator<CreateRentRequest>
{

    public CreatePaymentIntentRequestValidator()
    {
        RuleFor(x => x.CarId).NotEmpty().WithMessage("Car Id cannot be empty").NotNull()
            .WithMessage("Car Id cannot be null").NotEqual(Guid.Empty)
            .WithMessage("The value sent to Car Id is not acceptable");
        RuleFor(x => x.StartDate).NotEmpty().WithMessage("Start Date cannot be empty").NotNull()
            .WithMessage("Start Date cannot be null").LessThan(DateTime.UtcNow);
        RuleFor(x => x.EndDate).NotEmpty().WithMessage("End Date cannot be empty").NotNull()
            .WithMessage("End Date cannot be null").LessThan(DateTime.UtcNow);
    }
}
