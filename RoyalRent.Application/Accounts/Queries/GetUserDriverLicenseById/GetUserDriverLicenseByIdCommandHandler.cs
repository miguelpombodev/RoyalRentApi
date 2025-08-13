using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Domain.Abstractions;
using RoyalRent.Domain.Entities;
using RoyalRent.Domain.Errors;

namespace RoyalRent.Application.Accounts.Queries.GetUserDriverLicenseById;

public class
    GetUserDriverLicenseByIdCommandHandler : ICommandHandler<GetUserDriverLicenseByIdCommand, Result<UserDriverLicense>>
{
    private readonly IAccountRepository _accountRepository;

    public GetUserDriverLicenseByIdCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Result<UserDriverLicense>> Handle(GetUserDriverLicenseByIdCommand request,
        CancellationToken cancellationToken)
    {
        var userDriverLicense = await _accountRepository.GetDriverLicense(request.Id);

        if (userDriverLicense is null)
            return Result<UserDriverLicense>.Failure(DriverLicenseErrors.UserDriverLicenseNotFound);

        return Result<UserDriverLicense>.Success(userDriverLicense);
    }
}
