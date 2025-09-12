using AutoMapper;
using Microsoft.Extensions.Logging;
using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Domain.Abstractions;
using RoyalRent.Domain.Entities;
using RoyalRent.Domain.Users.Entities;

namespace RoyalRent.Application.Accounts.Commands.CreateDriverLicense;

public class CreateDriverLicenseCommandHandler : ICommandHandler<CreateDriverLicenseCommand, Result<string>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unit;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateDriverLicenseCommandHandler> _logger;

    public CreateDriverLicenseCommandHandler(IAccountRepository accountRepository, IUnitOfWork unit, IMapper mapper,
        ILogger<CreateDriverLicenseCommandHandler> logger, IPasswordHasherProvider passwordHasher)
    {
        _accountRepository = accountRepository;
        _unit = unit;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<string>> Handle(CreateDriverLicenseCommand request, CancellationToken cancellationToken)
    {
        var mappedDriverLicense = _mapper.Map<UserDriverLicense>(request);

        await _accountRepository.AddDriverLicense(mappedDriverLicense);

        await _unit.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Added Driver License from User {UserId}", request.UserId);

        return Result<string>.Success("Driver license created");
    }
}
