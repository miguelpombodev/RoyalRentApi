using RoyalRent.Application.DTOs;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Abstractions.Accounts;

public interface ICreateDriverLicenseService
{
    Task<Result<string>> ExecuteAsync(CreateUserDriverLicenseDto dto, Guid userId);
}
