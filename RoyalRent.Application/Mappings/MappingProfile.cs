using AutoMapper;
using Mapster;
using Microsoft.AspNetCore.Routing.Constraints;
using RoyalRent.Application.Accounts.Commands.CreateDriverLicense;
using RoyalRent.Application.Cars.Commands.CreateCarsDataByCsvFile;
using RoyalRent.Application.Cars.Model;
using RoyalRent.Application.DTOs;
using RoyalRent.Application.DTOs.Inputs;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Mappings;

public static class MapsterConfig
{
    public static void ConfigureMappings()
    {
        TypeAdapterConfig.GlobalSettings.Scan(typeof(MapsterConfig).Assembly);
    }

    public class DriverLicenseMappingProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(CreateUserDriverLicenseDto dto, string userEmail), CreateDriverLicenseCommand>()
                .MapWith(data => new CreateDriverLicenseCommand(
                    data.dto.Rg,
                    data.dto.BirthDate,
                    data.dto.DriverLicenseNumber,
                    data.dto.DocumentExpirationDate,
                    data.dto.State,
                    null,
                    data.userEmail
                ));
        }
    }
}
