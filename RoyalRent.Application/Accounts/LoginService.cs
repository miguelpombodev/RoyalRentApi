using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Accounts;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Application.Accounts.Errors;
using RoyalRent.Application.DTOs;
using RoyalRent.Application.DTOs.Outputs;
using RoyalRent.Application.Repositories;

namespace RoyalRent.Application.Accounts;

public class LoginService : ILoginService
{
    private readonly IAuthenticationProvider _authProvider;

    public LoginService(IAuthenticationProvider authProvider)
    {
        _authProvider = authProvider;
    }

    public async Task<Result<AuthResult>> ExecuteAsync(LoginDto account)
    {
        var result = await _authProvider.AuthenticateAsync(account);


        return result;
    }
}
