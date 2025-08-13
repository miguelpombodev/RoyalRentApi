using Microsoft.AspNetCore.Http;
using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;

namespace RoyalRent.Application.Cars.Commands.CreateCarsDataByCsvFile;

public record CreateCarsDataByCsvFileCommand(IFormFile carsCsvFile) : ICommand<Result<string>>;
