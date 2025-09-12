using Microsoft.Extensions.Logging;
using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Domain.Abstractions;
using RoyalRent.Domain.Entities;
using RoyalRent.Domain.Errors;
using RoyalRent.Domain.Users.Entities;

namespace RoyalRent.Application.Accounts.Queries.GetById;

public sealed class GetByIdCommandHandler : ICommandHandler<GetByIdCommand, Result<User>>
{
    private readonly IAccountRepository _accountRepository;

    public GetByIdCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Result<User>> Handle(GetByIdCommand request, CancellationToken cancellationToken)
    {
        var user = await _accountRepository.GetUserBasicInformationById(request.id);

        if (user is null) return Result<User>.Failure(AccountErrors.UserAccountNotFound);

        return Result<User>.Success(user);
    }
}
