using RoyalRent.Application.DTOs;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Abstractions.Accounts;

public interface ICreateAccountService
{
    Task<User> ExecuteAsync(CreateAccountDto account);
}
