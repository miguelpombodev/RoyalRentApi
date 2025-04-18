using AutoMapper;
using RoyalRent.Application.Abstractions.Accounts;
using RoyalRent.Application.DTOs;
using RoyalRent.Application.Repositories;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Accounts;

public class CreateAccountService : ICreateAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unit;
    private readonly IMapper _mapper;

    public CreateAccountService(IMapper mapper, IAccountRepository accountRepository, IUnitOfWork unit)
    {
        _mapper = mapper;
        _accountRepository = accountRepository;
        _unit = unit;
    }
    public async Task<User> ExecuteAsync(CreateAccountDto account)
    {
        var user = _mapper.Map<User>(account);

        var addedAcount = await _accountRepository.AddAccount(user);

        await _unit.SaveChangesAsync();

        return addedAcount;
    }
}
