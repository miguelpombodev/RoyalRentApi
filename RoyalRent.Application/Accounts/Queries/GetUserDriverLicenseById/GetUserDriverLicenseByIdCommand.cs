using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Accounts.Queries.GetUserDriverLicenseById;

public sealed record GetUserDriverLicenseByIdCommand(Guid Id) : ICommand<Result<UserDriverLicense>>;
