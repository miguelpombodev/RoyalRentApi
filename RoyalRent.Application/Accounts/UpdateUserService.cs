using Microsoft.Extensions.Logging;
using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Accounts;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Application.Accounts.Errors;
using RoyalRent.Application.Repositories;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Accounts;

public class UpdateUserService : IUpdateUserService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unit;
    private readonly ILogger<UpdateUserService> _logger;
    private readonly IPasswordHasherProvider _passwordHasher;


    public UpdateUserService(IAccountRepository accountRepository, ILogger<UpdateUserService> logger,
        IPasswordHasherProvider passwordHasher,
        IUnitOfWork unit)
    {
        _accountRepository = accountRepository;
        _passwordHasher = passwordHasher;
        _logger = logger;
        _unit = unit;
    }

    public async Task<Result<string>> UpdateUserPassword(Guid id, string newPassword)
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
