using Microsoft.AspNetCore.Http;

namespace RoyalRent.Application.Abstractions.Providers;

public interface ICsvProvider
{
    IEnumerable<T> ReadCsvFile<T>(IFormFile file);
}
