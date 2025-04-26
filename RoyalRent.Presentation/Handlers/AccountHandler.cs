using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Accounts;
using RoyalRent.Application.DTOs;
using RoyalRent.Domain.Entities;
using RoyalRent.Presentation.Abstractions;
using RoyalRent.Presentation.Accounts.Requests;

namespace RoyalRent.Presentation.Handlers;

public class AccountHandler : IAccountHandler
{
    private readonly ICreateAccountService _createAccountService;
    private readonly IGetUserBasicInformationService _getUserBasicInformationService;
    private readonly ICreateDriverLicenseService _addDriverLicenseService;

    public AccountHandler(ICreateAccountService createAccountService,
        IGetUserBasicInformationService getUserBasicInformationService,
        ICreateDriverLicenseService addDriverLicenseService
    )
    {
        _createAccountService = createAccountService;
        _getUserBasicInformationService = getUserBasicInformationService;
        _addDriverLicenseService = addDriverLicenseService;
    }

    public async Task SaveAccountAsync(CreateAccountDto request)
    {
        await _createAccountService.ExecuteAsync(request);
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
}
