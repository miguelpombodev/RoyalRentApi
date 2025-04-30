using RoyalRent.Application.DTOs;
using RoyalRent.Application.DTOs.Outputs;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Abstractions.Accounts;

public interface ILoginService
{
    Task<Result<AuthResult>> ExecuteAsync(LoginDto account);
}
