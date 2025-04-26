using RoyalRent.Application.Abstractions;
using RoyalRent.Application.DTOs;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Presentation.Abstractions;

public interface IAccountHandler
{
    Task SaveAccountAsync(CreateAccountDto request);
    Task<Result<User>> GetUserInformationAsync(Guid id);
    Task<Result<string>> SaveDriverLicense(CreateUserDriverLicenseDto request, Guid userId);
}
