using RoyalRent.Application.DTOs;
using RoyalRent.Application.DTOs.Inputs;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Abstractions.Accounts;

public interface IAccountCommandService
{
    Task<Result<User>> ExecuteCreateAccountService(CreateAccountDto account);
    Task<Result<string>> ExecuteCreateDriverLicenseService(CreateUserDriverLicenseDto dto, string userEmail);
    Task<Result<string>> ExecuteUpdateUserPasswordService(Guid id, string newPassword);
}
