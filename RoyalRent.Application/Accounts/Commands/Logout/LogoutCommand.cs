using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;

namespace RoyalRent.Application.Accounts.Commands.Logout;

public sealed record LogoutCommand(string RefreshToken) : ICommand<Result<bool>>;
