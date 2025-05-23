using RoyalRent.Application.DTOs;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Abstractions.Accounts;

public interface ICreateAccountService
{
    Task<Result<User>> ExecuteAsync(CreateAccountDto account);
}
