using AutoMapper;
using Microsoft.Extensions.Logging;
using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Domain.Abstractions;
using RoyalRent.Domain.Entities;
using RoyalRent.Domain.Errors;

namespace RoyalRent.Application.Accounts.Commands.CreateAccount;

public sealed class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand, Result<User>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unit;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateAccountCommandHandler> _logger;

    public CreateAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unit, IMapper mapper,
        ILogger<CreateAccountCommandHandler> logger, IPasswordHasherProvider passwordHasher)
    {
        _accountRepository = accountRepository;
        _unit = unit;
        _mapper = mapper;
        _logger = logger;
        _passwordHasher = passwordHasher;
    }

    private readonly IPasswordHasherProvider _passwordHasher;

    public async Task<Result<User>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var checkIfUserExists = await _accountRepository.GetUserByEmail(request.Email);

        if (checkIfUserExists is not null)
        {
            _logger.LogError("There was a attempt to log with a registered email account: {UserEmail}", request.Email);
            return Result<User>.Failure(AccountErrors.UserAccountAlreadyRegistered);
        }

        var user = _mapper.Map<User>(request);

        var passwordHashed = _passwordHasher.Hash(request.Password);

        var userPassword = new UserPassword(passwordHashed, user.Id, true);

        var addedAccount = await _accountRepository.AddAccount(user, userPassword);

        await _unit.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User {UserEmail} was successfully registered", addedAccount.Item1.Email);
        _logger.LogInformation("User Password {CreatedUserPasswordId} was successfully registered",
            addedAccount.Item2.Id);

        return Result<User>.Success(addedAccount.Item1);
    }
}
