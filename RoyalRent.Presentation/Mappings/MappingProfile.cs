using AutoMapper;
using RoyalRent.Application.DTOs;
using RoyalRent.Domain.Entities;
using RoyalRent.Presentation.Accounts.Requests;
using RoyalRent.Presentation.Accounts.Responses;

namespace RoyalRent.Presentation.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateAccountRequest, CreateAccountDto>().ConstructUsing(body => new CreateAccountDto(
            body.Name, body.Cpf, body.Email, body.Telephone, body.Gender
        ));
        CreateMap<User, GetUserResponse>().ConstructUsing(user =>
            new GetUserResponse(user.Name, user.Cpf, user.Email, user.Telephone, user.Gender));
    }
}
