using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Accounts;
using RoyalRent.Application.Accounts.Errors;
using RoyalRent.Application.DTOs;
using RoyalRent.Application.Repositories;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Accounts;

public class GetUserService : IGetUserService
{
    private readonly IAccountRepository _accountRepository;

    public GetUserService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }


    public async Task<Result<User>> ExecuteGetByIdAsync(Guid id)
    {
        var user = await _accountRepository.GetUserBasicInformationById(id);

        if (user is null) return Result<User>.Failure(AccountErrors.UserAccountNotFound);

        return Result<User>.Success(user);
    }

    public async Task<Result<User>> ExecuteGetByEmailAsync(string email)
    {
        var user = await _accountRepository.GetUserByEmail(email);

        if (user is null) return Result<User>.Failure(AccountErrors.UserAccountNotFound);

        return Result<User>.Success(user);
    }
}
