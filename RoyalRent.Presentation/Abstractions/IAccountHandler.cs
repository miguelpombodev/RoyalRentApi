using RoyalRent.Application.Abstractions;
using RoyalRent.Application.DTOs;
using RoyalRent.Application.DTOs.Inputs;
using RoyalRent.Application.DTOs.Outputs;
using RoyalRent.Domain.Entities;
using RoyalRent.Presentation.Accounts.Requests;

namespace RoyalRent.Presentation.Abstractions;

public interface IAccountHandler
{
    Task<Result<User>> SaveAccountHandler(CreateAccountDto request);
    Task<Result<User>> GetUserInformationHandler(string email);
    Task<Result<string>> SaveDriverLicenseHandler(CreateUserDriverLicenseDto request, string userEmail);
    Task<Result<AuthResult>> LoginHandler(LoginAccountRequest body);
    Task<Result<AuthResult>> GenerateRefreshTokenHandler(string refreshToken);
    Task<Result<bool>> LogoutHandler(string refreshToken);
    Task<Result<UserDriverLicense>> GetUserDriverLicenseHandler(string email);

}
