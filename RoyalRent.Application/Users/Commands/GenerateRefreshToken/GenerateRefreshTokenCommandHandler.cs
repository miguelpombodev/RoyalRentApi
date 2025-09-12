using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Application.DTOs.Outputs;

namespace RoyalRent.Application.Accounts.Commands.GenerateRefreshToken;

public sealed class
    GenerateRefreshTokenCommandHandler : ICommandHandler<GenerateRefreshTokenCommand, Result<AuthResult>>
{
    private readonly IAuthenticationProvider _authProvider;

    public GenerateRefreshTokenCommandHandler(IAuthenticationProvider authProvider)
    {
        _authProvider = authProvider;
    }

    public async Task<Result<AuthResult>> Handle(GenerateRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var result = await _authProvider.RefreshTokenAsync(request.RefreshToken);

        return result;
    }
}
