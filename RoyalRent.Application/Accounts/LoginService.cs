using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Accounts;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Application.Accounts.Errors;
using RoyalRent.Application.DTOs;
using RoyalRent.Application.Repositories;

namespace RoyalRent.Application.Accounts;

public class LoginService : ILoginService
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordHasherProvider _passwordHasher;

    public LoginService(ITokenProvider tokenProvider, IAccountRepository accountRepository,
        IPasswordHasherProvider passwordHasher)
    {
        _tokenProvider = tokenProvider;
        _accountRepository = accountRepository;
        _passwordHasher = passwordHasher;
    }


    public async Task<Result<string>> ExecuteAsync(LoginDto account)
    {
        var checkIfUserExists = await _accountRepository.GetUserByEmail(account.Email);

        if (checkIfUserExists is null) return Result<string>.Failure(AccountErrors.UserAccountNotFound);

        var userActualPassword = await _accountRepository.GetLastActualUserPassword(checkIfUserExists.Id);

        if (userActualPassword is null)
            return Result<string>.Failure(AccountErrors.UserAccountPasswordDoesNotExist);

        if (!_passwordHasher.Verify(account.Password, userActualPassword.PasswordHashed))
            return Result<string>.Failure(AccountErrors.UserAccountPasswordDoesNotMatch);

        var token = _tokenProvider.Create(checkIfUserExists);

        return Result<string>.Success(token);
    }
}
