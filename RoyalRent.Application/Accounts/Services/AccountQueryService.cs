using AutoMapper;
using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Accounts;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Application.Accounts.Errors;
using RoyalRent.Application.DTOs;
using RoyalRent.Application.DTOs.Outputs;
using RoyalRent.Application.Repositories;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Accounts.Services;

public class AccountQueryService : IAccountQueryService
{
    private readonly IAuthenticationProvider _authProvider;
    private readonly IAccountRepository _accountRepository;


    public AccountQueryService(IAuthenticationProvider authProvider, IAccountRepository accountRepository)
    {
        _authProvider = authProvider;
        _accountRepository = accountRepository;
    }

    public async Task<Result<AuthResult>> ExecuteLoginService(LoginDto account)
    {
        var result = await _authProvider.AuthenticateAsync(account);


        return result;
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

    public async Task<Result<UserDriverLicense>> ExecuteGetUserDriverLicenseByIdAsync(Guid id)
    {
        var userDriverLicense = await _accountRepository.GetDriverLicense(id);

        if (userDriverLicense is null)
            return Result<UserDriverLicense>.Failure(DriverLicenseErrors.UserDriverLicenseNotFound);

        return Result<UserDriverLicense>.Success(userDriverLicense);
    }
}
