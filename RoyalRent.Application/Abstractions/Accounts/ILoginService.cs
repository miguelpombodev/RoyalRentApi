using RoyalRent.Application.DTOs;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Abstractions.Accounts;

public interface ILoginService
{
    Task<Result<string>> ExecuteAsync(LoginDto account);
}
