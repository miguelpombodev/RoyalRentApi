using AutoMapper;
using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Accounts;
using RoyalRent.Application.DTOs;
using RoyalRent.Application.Repositories;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Accounts;

public class CreateDriverLicenseService : ICreateDriverLicenseService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateDriverLicenseService(IAccountRepository accountRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<string>> ExecuteAsync(CreateUserDriverLicenseDto dto, Guid userId)
    {
        var mappedDriverLicense = _mapper.Map<UserDriverLicense>(dto);

        mappedDriverLicense.Id = userId;

        await _accountRepository.AddDriverLicense(mappedDriverLicense);

        await _unitOfWork.SaveChangesAsync();

        return Result<string>.Success("Driver license created");
    }
}
