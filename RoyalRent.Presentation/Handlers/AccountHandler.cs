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

    public AccountHandler(ICreateAccountService createAccountService,
        IGetUserBasicInformationService getUserBasicInformationService)
    {
        _createAccountService = createAccountService;
        _getUserBasicInformationService = getUserBasicInformationService;
    }
    public async Task SaveAccountAsync(CreateAccountDto request)
    {
        await _createAccountService.ExecuteAsync(request);
    }

    public async Task<User?> GetUserInformationAsync(Guid id)
    {
        var user = await _getUserBasicInformationService.ExecuteGetByIdAsync(id);

        return user;
    }
}
