using Microsoft.Extensions.Logging;
using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Application.DTOs.Outputs;

namespace RoyalRent.Application.Accounts.Queries.Login;

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, Result<AuthResult>>
{
    private readonly IAuthenticationProvider _authProvider;
    private readonly ILogger<LoginCommandHandler> _logger;


    public LoginCommandHandler(IAuthenticationProvider authProvider, ILogger<LoginCommandHandler> logger)
    {
        _authProvider = authProvider;
        _logger = logger;
    }

    public async Task<Result<AuthResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _authProvider.AuthenticateAsync(request.Email, request.Password);

        _logger.LogInformation("User Email {Email} Account is logged!", request.Email);

        return result;
    }
}
