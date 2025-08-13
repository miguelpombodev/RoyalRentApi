using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;

namespace RoyalRent.Application.Accounts.Commands.UpdateUserPassword;

public sealed record UpdateUserPasswordCommand(Guid id, string newPassword) : ICommand<Result<string>>;
