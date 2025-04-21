using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Accounts;
using RoyalRent.Application.Accounts.Errors;
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


    public async Task<Result<User>> ExecuteGetByIdAsync(Guid id)
    {
        var user = await _accountRepository.GetUserBasicInformationById(id);

        if (user is null) return Result<User>.Failure(AccountErrors.UserAccountNotFound);

        return Result<User>.Success(user);
    }
}
