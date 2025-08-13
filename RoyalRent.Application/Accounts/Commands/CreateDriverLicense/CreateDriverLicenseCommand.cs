using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;

namespace RoyalRent.Application.Accounts.Commands.CreateDriverLicense;

public sealed record CreateDriverLicenseCommand(
    string Rg,
    DateOnly BirthDate,
    string DriverLicenseNumber,
    DateOnly DocumentExpirationDate,
    string State,
    Guid? UserId,
    string UserEmail) : ICommand<Result<string>>;
