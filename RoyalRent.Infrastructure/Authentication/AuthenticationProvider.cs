using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Application.DTOs.Outputs;
using RoyalRent.Domain.Abstractions;
using RoyalRent.Domain.Errors;

namespace RoyalRent.Infrastructure.Authentication;

public class AuthenticationProvider : IAuthenticationProvider
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITokenProvider _tokenProvider;
    private readonly IPasswordHasherProvider _passwordHasher;

    public AuthenticationProvider(IAccountRepository accountRepository, ITokenProvider tokenProvider,
        IPasswordHasherProvider passwordHasher)
    {
        _accountRepository = accountRepository;
        _tokenProvider = tokenProvider;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<AuthResult>> AuthenticateAsync(string email, string password)
    {
        var user = await _accountRepository.GetUserByEmail(email);

        if (user is null)
        {
            return Result<AuthResult>.Failure(AccountErrors.UserAccountNotFound);
        }

        var userPassword = await _accountRepository.GetLastActualUserPassword(user.Id);

        if (userPassword is null || !_passwordHasher.Verify(password, userPassword.PasswordHashed))
            return Result<AuthResult>.Failure(AccountErrors.UserAccountPasswordDoesNotMatch);

        var accessToken = _tokenProvider.Create(user);
        var refreshToken = _tokenProvider.CreateRefreshToken();

        await _tokenProvider.ReplaceRefreshTokenAsync(user.Id, refreshToken);

        var auth = AuthResult.Create(accessToken, refreshToken);

        return Result<AuthResult>.Success(auth);
    }

    public async Task<Result<AuthResult>> RefreshTokenAsync(string refreshToken)
    {
        var result = await _tokenProvider.ValidateRefreshTokenAsync(refreshToken);

        if (result.IsFailure)
            return Result<AuthResult>.Failure(result.Error);

        var user = await _accountRepository.GetUserBasicInformationById(result.Data);

        if (user is null)
        {
            return Result<AuthResult>.Failure(AccountErrors.UserAccountNotFound);
        }

        var newAccessToken = _tokenProvider.Create(user);
        var newRefreshToken = _tokenProvider.CreateRefreshToken();

        await _tokenProvider.ReplaceRefreshTokenAsync(user.Id, newRefreshToken);

        var auth = AuthResult.Create(newAccessToken, newRefreshToken);

        return Result<AuthResult>.Success(auth);
    }

    public async Task<Result<bool>> RevokeRefreshTokenAsync(string refreshToken)
    {
        var result = await _tokenProvider.DeleteRefreshTokenAsync(refreshToken);

        return !result
            ? Result<bool>.Failure(AuthenticationErrors.DeleteRefreshTokenError)
            : Result<bool>.Success(result);
    }
}
