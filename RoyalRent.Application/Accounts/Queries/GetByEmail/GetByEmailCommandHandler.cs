using Microsoft.Extensions.Logging;
using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Domain.Abstractions;
using RoyalRent.Domain.Entities;
using RoyalRent.Domain.Errors;

namespace RoyalRent.Application.Accounts.Queries.GetByEmail;

public sealed class GetByEmailCommandHandler : ICommandHandler<GetByEmailCommand, Result<User>>
{
    private readonly IAccountRepository _accountRepository;


    public GetByEmailCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Result<User>> Handle(GetByEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _accountRepository.GetUserByEmail(request.Email);

        if (user is null) return Result<User>.Failure(AccountErrors.UserAccountNotFound);

        return Result<User>.Success(user);
    }
}
