using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Application.DTOs.Outputs;
using RoyalRent.Application.Users.DTOs;

namespace RoyalRent.Application.Accounts.Commands.GenerateRefreshToken;

public sealed record GenerateRefreshTokenCommand(string RefreshToken) : ICommand<Result<AuthResult>>;
