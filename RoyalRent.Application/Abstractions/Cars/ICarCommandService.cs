using Microsoft.AspNetCore.Http;

namespace RoyalRent.Application.Abstractions.Cars;

public interface ICarCommandService
{
    Task<Result<string>> InsertCarsDataByCsvFile(IFormFile carsCsvFile);
}
