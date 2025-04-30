using RoyalRent.Application.Abstractions;
using RoyalRent.Application.DTOs;
using RoyalRent.Application.DTOs.Outputs;
using RoyalRent.Domain.Entities;
using RoyalRent.Presentation.Accounts.Requests;

namespace RoyalRent.Presentation.Abstractions;

public interface IAccountHandler
{
    Task<Result<User>> SaveAccountAsync(CreateAccountDto request);
    Task<Result<User>> GetUserInformationAsync(Guid id);
    Task<Result<string>> SaveDriverLicense(CreateUserDriverLicenseDto request, Guid userId);
    Task<Result<AuthResult>> Login(LoginAccountRequest body);
    Task<Result<AuthResult>> GenerateRefreshTokenHandler(string refreshToken);
    Task<Result<bool>> LogoutHandler(string refreshToken);

}
