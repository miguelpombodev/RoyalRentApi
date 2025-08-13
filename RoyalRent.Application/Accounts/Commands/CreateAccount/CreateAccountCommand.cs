using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Accounts.Commands.CreateAccount;

public sealed record CreateAccountCommand(
    string Name,
    string Cpf,
    string Email,
    string Telephone,
    char? Gender,
    string Password) : ICommand<Result<User>>;
