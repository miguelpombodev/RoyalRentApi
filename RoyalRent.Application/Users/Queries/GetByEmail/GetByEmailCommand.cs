using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Domain.Entities;
using RoyalRent.Domain.Users.Entities;

namespace RoyalRent.Application.Accounts.Queries.GetByEmail;

public sealed record GetByEmailCommand(string Email) : ICommand<Result<User>>;
