using AutoMapper;
using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Accounts;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Application.Accounts.Errors;
using RoyalRent.Application.DTOs;
using RoyalRent.Application.Repositories;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Accounts;

public class CreateAccountService : ICreateAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unit;
    private readonly IMapper _mapper;

    private readonly IPasswordHasherProvider _passwordHasher;

    public CreateAccountService(IMapper mapper, IAccountRepository accountRepository, IUnitOfWork unit,
        IPasswordHasherProvider passwordHasher)
    {
        _mapper = mapper;
        _accountRepository = accountRepository;
        _unit = unit;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<User>> ExecuteAsync(CreateAccountDto account)
    {
        var checkIfUserExists = await _accountRepository.GetUserByEmail(account.Email);

        if (checkIfUserExists is not null) return Result<User>.Failure(AccountErrors.UserAccountAlreadyRegistered);

        var user = _mapper.Map<User>(account);

        var passwordHashed = _passwordHasher.Hash(account.password);

        var userPassword = new UserPassword(passwordHashed, user.Id, true);

        var addedAccount = await _accountRepository.AddAccount(user, userPassword);

        await _unit.SaveChangesAsync();

        return Result<User>.Success(addedAccount.Item1);
    }
}
