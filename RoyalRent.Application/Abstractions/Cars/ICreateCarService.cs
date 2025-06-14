using Microsoft.AspNetCore.Http;

namespace RoyalRent.Application.Abstractions.Cars;

public interface ICreateCarService
{
    Task<Result<string>> InsertCarsDataByCsvFile(IFormFile carsCsvFile);
}
