using FluentValidation;
using RoyalRent.Application.DTOs.Inputs;

namespace RoyalRent.Application.Behaviors.Validators;

public class InsertFromCsvFileValidator : AbstractValidator<InsertFromCsvFileRequest>
{
    public InsertFromCsvFileValidator()
    {
        RuleFor(x => x.File).NotNull().WithMessage("CSV File cannot be null or empty").NotEmpty()
            .WithMessage("CSV File cannot be null or empty").Must(file => file.FileName.EndsWith(".csv"))
            .WithMessage("The provided file must be a CSV file");
    }
}
