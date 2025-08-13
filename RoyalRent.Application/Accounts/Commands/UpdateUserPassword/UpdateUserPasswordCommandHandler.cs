using AutoMapper;
using Microsoft.Extensions.Logging;
using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Domain.Abstractions;
using RoyalRent.Domain.Entities;
using RoyalRent.Domain.Errors;

namespace RoyalRent.Application.Accounts.Commands.UpdateUserPassword;

public class UpdateUserPasswordCommandHandler : ICommandHandler<UpdateUserPasswordCommand, Result<string>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unit;
    private readonly ILogger<UpdateUserPasswordCommandHandler> _logger;

    public UpdateUserPasswordCommandHandler(IAccountRepository accountRepository, IUnitOfWork unit,
        ILogger<UpdateUserPasswordCommandHandler> logger, IPasswordHasherProvider passwordHasher)
    {
        _accountRepository = accountRepository;
        _unit = unit;
        _logger = logger;
        _passwordHasher = passwordHasher;
    }

    private readonly IPasswordHasherProvider _passwordHasher;

    public async Task<Result<string>> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var lastPassword = await _accountRepository.GetLastActualUserPassword(request.id);

        if (lastPassword is null)
        {
            return Result<string>.Failure(AccountErrors.UserAccountPasswordDoesNotMatch);
        }

        if (_passwordHasher.Verify(request.newPassword, lastPassword.PasswordHashed))
        {
            return Result<string>.Failure(AccountErrors.UserAccountPasswordIsEqualThanLastOne);
        }

        var passwordHashed = _passwordHasher.Hash(request.newPassword);

        var userPassword = new UserPassword(passwordHashed, request.id, true);

        await _accountRepository.UpdateAccountPassword(request.id, userPassword, lastPassword.Id);

        await _unit.SaveChangesAsync();

        _logger.LogInformation("User {Id} has changed its password", request.id);

        return Result<string>.Success("success!");
    }
}
