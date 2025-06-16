using RoyalRent.Application.DTOs;
using RoyalRent.Application.DTOs.Outputs;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Abstractions.Accounts;

public interface IAccountQueryService
{
    Task<Result<AuthResult>> ExecuteLoginService(LoginDto account);
    Task<Result<User>> ExecuteGetByIdAsync(Guid id);
    Task<Result<User>> ExecuteGetByEmailAsync(string email);
    Task<Result<UserDriverLicense>> ExecuteGetUserDriverLicenseByIdAsync(Guid id);

}
