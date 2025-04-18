using AutoMapper;
using RoyalRent.Application.DTOs;
using RoyalRent.Presentation.Accounts.Requests;

namespace RoyalRent.Presentation.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateAccountRequest, CreateAccountDto>().ConstructUsing(body => new CreateAccountDto(
            body.Name, body.Cpf, body.Email, body.Telephone, body.Gender
        ));
    }
}
