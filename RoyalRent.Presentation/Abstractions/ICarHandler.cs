using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Cars.DTOs;
using RoyalRent.Presentation.Cars.Requests;

namespace RoyalRent.Presentation.Abstractions;

public interface ICarHandler
{
    Task<Result<string>> InsertCarsDataByCsvFile(InsertFromCsvFileRequest body);
}
