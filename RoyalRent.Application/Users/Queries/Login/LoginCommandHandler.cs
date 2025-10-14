using Microsoft.Extensions.Logging;
using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Application.Accounts.Queries.Login;
using RoyalRent.Application.Users.DTOs;

namespace RoyalRent.Application.Users.Queries.Login;

public sealed class LoginCommandHandler(IAuthenticationProvider authProvider, ILogger<LoginCommandHandler> logger)
    : ICommandHandler<LoginCommand, Result<AuthResult>>
{
    public async Task<Result<AuthResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await authProvider.AuthenticateAsync(request.Email, request.Password);

        logger.LogInformation("User Email {Email} Account is logged!", request.Email);

        return result;
    }
}
