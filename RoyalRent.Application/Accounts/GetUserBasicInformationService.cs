using RoyalRent.Application.Abstractions.Accounts;
using RoyalRent.Application.DTOs;
using RoyalRent.Application.Repositories;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Accounts;

public class GetUserBasicInformationService : IGetUserBasicInformationService
{
    private readonly IAccountRepository _accountRepository;

    public GetUserBasicInformationService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }


    public async Task<User?> ExecuteGetByIdAsync(Guid id)
    {
        var user = await _accountRepository.GetUserBasicInformationById(id);

        if (user is null)
        {
            return null;
        }

        return user;
    }
}
