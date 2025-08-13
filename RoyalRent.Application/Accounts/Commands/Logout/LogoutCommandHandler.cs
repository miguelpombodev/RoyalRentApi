using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Application.Abstractions.Providers;

namespace RoyalRent.Application.Accounts.Commands.Logout;

public sealed class LogoutCommandHandler : ICommandHandler<LogoutCommand, Result<bool>>
{
    private readonly IAuthenticationProvider _authProvider;

    public LogoutCommandHandler(IAuthenticationProvider authProvider)
    {
        _authProvider = authProvider;
    }

    public async Task<Result<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var result = await _authProvider.RevokeRefreshTokenAsync(request.RefreshToken);

        return result;
    }
}
