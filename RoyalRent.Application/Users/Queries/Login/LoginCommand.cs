using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Application.DTOs.Outputs;
using RoyalRent.Application.Users.DTOs;

namespace RoyalRent.Application.Accounts.Queries.Login;

public sealed record LoginCommand(
    string Name,
    string Cpf,
    string Email,
    string Telephone,
    char? Gender,
    string Password) : ICommand<Result<AuthResult>>;
