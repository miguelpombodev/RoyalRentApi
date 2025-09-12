using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Domain.Entities;
using RoyalRent.Domain.Users.Entities;

namespace RoyalRent.Application.Accounts.Queries.GetById;

public sealed record GetByIdCommand(Guid id) : ICommand<Result<User>>;
