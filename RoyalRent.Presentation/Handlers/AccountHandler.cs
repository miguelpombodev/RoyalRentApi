using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Accounts;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Application.DTOs;
using RoyalRent.Application.DTOs.Outputs;
using RoyalRent.Domain.Entities;
using RoyalRent.Presentation.Abstractions;
using RoyalRent.Presentation.Accounts.Requests;

namespace RoyalRent.Presentation.Handlers;

public class AccountHandler : IAccountHandler
{
    private readonly ICreateAccountService _createAccountService;
    private readonly IGetUserBasicInformationService _getUserBasicInformationService;
    private readonly ICreateDriverLicenseService _addDriverLicenseService;
    private readonly ILoginService _loginService;
    private readonly IAuthenticationProvider _authProvider;

    public AccountHandler(ICreateAccountService createAccountService,
        IGetUserBasicInformationService getUserBasicInformationService,
        ICreateDriverLicenseService addDriverLicenseService,
        ILoginService loginService,
        IAuthenticationProvider authProvider
    )
    {
        _createAccountService = createAccountService;
        _getUserBasicInformationService = getUserBasicInformationService;
        _addDriverLicenseService = addDriverLicenseService;
        _loginService = loginService;
        _authProvider = authProvider;
    }

    public async Task<Result<User>> SaveAccountAsync(CreateAccountDto request)
    {
        return await _createAccountService.ExecuteAsync(request);
    }

    public async Task<Result<User>> GetUserInformationAsync(Guid id)
    {
        var result = await _getUserBasicInformationService.ExecuteGetByIdAsync(id);

        return result;
    }

    public async Task<Result<string>> SaveDriverLicense(CreateUserDriverLicenseDto request, Guid userId)
    {
        var result = await _addDriverLicenseService.ExecuteAsync(request, userId);

        return result;
    }

    public async Task<Result<AuthResult>> Login(LoginAccountRequest body)
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
}
