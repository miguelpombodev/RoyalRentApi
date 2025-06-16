using AutoMapper;
using Microsoft.Extensions.Logging;
using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Accounts;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Application.Accounts.Errors;
using RoyalRent.Application.DTOs;
using RoyalRent.Application.DTOs.Inputs;
using RoyalRent.Application.Repositories;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Accounts;

public class AccountCommandService : IAccountCommandService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unit;
    private readonly IMapper _mapper;
    private readonly ILogger<AccountCommandService> _logger;

    public AccountCommandService(IAccountRepository accountRepository, IUnitOfWork unit, IMapper mapper,
        ILogger<AccountCommandService> logger, IPasswordHasherProvider passwordHasher)
    {
        _accountRepository = accountRepository;
        _unit = unit;
        _mapper = mapper;
        _logger = logger;
        _passwordHasher = passwordHasher;
    }

    private readonly IPasswordHasherProvider _passwordHasher;

    public async Task<Result<User>> ExecuteCreateAccountService(CreateAccountDto account)
    {
        var checkIfUserExists = await _accountRepository.GetUserByEmail(account.Email);

        if (checkIfUserExists is not null)
        {
            _logger.LogError("There was a attempt to log with a registered email account: {UserEmail}", account.Email);
            return Result<User>.Failure(AccountErrors.UserAccountAlreadyRegistered);
        }

        var user = _mapper.Map<User>(account);

        var passwordHashed = _passwordHasher.Hash(account.Password);

        var userPassword = new UserPassword(passwordHashed, user.Id, true);

        var addedAccount = await _accountRepository.AddAccount(user, userPassword);

        await _unit.SaveChangesAsync();

        _logger.LogInformation("User {UserEmail} was successfully registered", addedAccount.Item1.Email);
        _logger.LogInformation("User Password {CreatedUserPasswordId} was successfully registered", addedAccount.Item2.Id);

        return Result<User>.Success(addedAccount.Item1);
    }

    public async Task<Result<string>> ExecuteCreateDriverLicenseService(CreateUserDriverLicenseDto dto,
        string userEmail)
    {
        dto.UserId = ((await _accountRepository.GetUserByEmail(userEmail))!).Id;
        var mappedDriverLicense = _mapper.Map<UserDriverLicense>(dto);

        await _accountRepository.AddDriverLicense(mappedDriverLicense);

        await _unit.SaveChangesAsync();

        return Result<string>.Success("Driver license created");
    }

    public async Task<Result<string>> ExecuteUpdateUserPasswordService(Guid id, string newPassword)
    {
        var lastPassword = await _accountRepository.GetLastActualUserPassword(id);

        if (lastPassword is null)
        {
            return Result<string>.Failure(AccountErrors.UserAccountPasswordDoesNotMatch);
        }

        if (_passwordHasher.Verify(newPassword, lastPassword.PasswordHashed))
        {
            return Result<string>.Failure(AccountErrors.UserAccountPasswordIsEqualThanLastOne);
        }

        var passwordHashed = _passwordHasher.Hash(newPassword);

        var userPassword = new UserPassword(passwordHashed, id, true);

        await _accountRepository.UpdateAccountPassword(id, userPassword, lastPassword.Id);

        await _unit.SaveChangesAsync();

        _logger.LogInformation("User {Id} has changed its password", id);

        return Result<string>.Success("success!");
    }
}
