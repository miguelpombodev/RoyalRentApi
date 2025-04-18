using AutoMapper;
using RoyalRent.Application.DTOs;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateAccountDto, User>().ConstructUsing(dto =>
            new User(dto.Name, dto.Cpf, dto.Email, dto.Telephone, dto.Gender));
    }
}
