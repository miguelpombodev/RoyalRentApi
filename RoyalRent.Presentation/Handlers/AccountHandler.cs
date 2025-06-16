using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Accounts;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Application.DTOs;
using RoyalRent.Application.DTOs.Inputs;
using RoyalRent.Application.DTOs.Outputs;
using RoyalRent.Domain.Entities;
using RoyalRent.Presentation.Abstractions;
using RoyalRent.Presentation.Accounts.Requests;

namespace RoyalRent.Presentation.Handlers;

public class AccountHandler : IAccountHandler
{
    private readonly IAccountCommandService _accountCommandService;
    private readonly IAccountQueryService _accountQueryService;
    private readonly IAuthenticationProvider _authProvider;

    public AccountHandler(
        IAuthenticationProvider authProvider,
        IAccountCommandService accountCommandService,
        IAccountQueryService accountQueryService)
    {
        _authProvider = authProvider;
        _accountCommandService = accountCommandService;
        _accountQueryService = accountQueryService;
    }

    public async Task<Result<User>> SaveAccountHandler(CreateAccountDto request)
    {
        return await _accountCommandService.ExecuteCreateAccountService(request);
    }

    public async Task<Result<User>> GetUserInformationHandler(string email)
    {
        var result = await _accountQueryService.ExecuteGetByEmailAsync(email);

        return result;
    }

    public async Task<Result<string>> SaveDriverLicenseHandler(CreateUserDriverLicenseDto request, string userEmail)
    {
        var result = await _accountCommandService.ExecuteCreateDriverLicenseService(request, userEmail);

        return result;
    }

    public async Task<Result<AuthResult>> LoginHandler(LoginAccountRequest body)
    {
        var result = await _accountQueryService.ExecuteLoginService(body);

        return result;
    }

    public async Task<Result<AuthResult>> GenerateRefreshTokenHandler(string refreshToken)
    {
        var result = await _authProvider.RefreshTokenAsync(refreshToken);

        return result;
    }

    public async Task<Result<bool>> LogoutHandler(string refreshToken)
    {
        var result = await _authProvider.RevokeRefreshTokenAsync(refreshToken);

        return result;
    }

    public async Task<Result<UserDriverLicense>> GetUserDriverLicenseHandler(string email)
    {
        var userResult = await _accountQueryService.ExecuteGetByEmailAsync(email);

        var userDriverLicenseResult =
            await _accountQueryService.ExecuteGetUserDriverLicenseByIdAsync(userResult.Data!.Id);

        return userDriverLicenseResult;
    }

    public async Task<Result<string>> UpdateUserForgotPasswordHandler(ForgotPasswordRequest body)
    {
        var userResult = await _accountQueryService.ExecuteGetByEmailAsync(body.email);

        var updateUserPasswordResult =
            await _accountCommandService.ExecuteUpdateUserPasswordService(userResult.Data!.Id, body.newPassword);

        return updateUserPasswordResult;
    }
}
