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
    private readonly ICreateAccountService _createAccountService;
    private readonly IGetUserService _getUserBasicInformationService;
    private readonly IUpdateUserService _updateUserService;
    private readonly ICreateDriverLicenseService _addDriverLicenseService;
    private readonly ILoginService _loginService;
    private readonly IAuthenticationProvider _authProvider;

    public AccountHandler(ICreateAccountService createAccountService,
        IGetUserService getUserBasicInformationService,
        IUpdateUserService updateUserService,
        ICreateDriverLicenseService addDriverLicenseService,
        ILoginService loginService,
        IAuthenticationProvider authProvider
    )
    {
        _createAccountService = createAccountService;
        _getUserBasicInformationService = getUserBasicInformationService;
        _addDriverLicenseService = addDriverLicenseService;
        _updateUserService = updateUserService;
        _loginService = loginService;
        _authProvider = authProvider;
    }

    public async Task<Result<User>> SaveAccountHandler(CreateAccountDto request)
    {
        return await _createAccountService.ExecuteAsync(request);
    }

    public async Task<Result<User>> GetUserInformationHandler(string email)
    {
        var result = await _getUserBasicInformationService.ExecuteGetByEmailAsync(email);

        return result;
    }

    public async Task<Result<string>> SaveDriverLicenseHandler(CreateUserDriverLicenseDto request, string userEmail)
    {
        var result = await _addDriverLicenseService.ExecuteAsync(request, userEmail);

        return result;
    }

    public async Task<Result<AuthResult>> LoginHandler(LoginAccountRequest body)
    {
        var result = await _loginService.ExecuteAsync(body);

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
        var userResult = await _getUserBasicInformationService.ExecuteGetByEmailAsync(email);

        var userDriverLicenseResult =
            await _getUserBasicInformationService.GetUserDriverLicenseByIdAsync(userResult.Data!.Id);

        return userDriverLicenseResult;
    }

    public async Task<Result<string>> UpdateUserForgotPasswordHandler(ForgotPasswordRequest body)
    {
        var userResult = await _getUserBasicInformationService.ExecuteGetByEmailAsync(body.email);

        var updateUserPasswordResult =
            await _updateUserService.UpdateUserPassword(userResult.Data!.Id, body.newPassword);

        return updateUserPasswordResult;
    }
}
