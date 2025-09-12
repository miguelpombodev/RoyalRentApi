using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RoyalRent.Application.Abstractions.Providers;

namespace RoyalRent.Infrastructure.Csv;

public class CsvProvider : ICsvProvider
{
    private readonly CsvConfiguration _csvConfiguration;
    private readonly ILogger<CsvProvider> _logger;

    public CsvProvider(ILogger<CsvProvider> logger)
    {
        _csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            HasHeaderRecord = true,
            PrepareHeaderForMatch = args => args.Header.Trim()
        };
        _logger = logger;
    }

    public IEnumerable<T> ReadCsvFile<T>(IFormFile file)
    {
        try
        {
            using var stream = file.OpenReadStream();

            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, _csvConfiguration);
            var records = csv.GetRecords<T>();
            return records.ToList();
        }
        catch (HeaderValidationException ex)
        {
            _logger.LogError(ex,
                "There was an error when trying to read CSV File Header - ExceptionMessage: {ExceptionMessage}",
                ex.Message);
            throw new ArgumentException("CSV File Header is invalid", ex);
        }
        catch (TypeConverterException ex)
        {
            _logger.LogError(ex,
                "The provided CSV file contains invalid data format - ExceptionMessage: {ExceptionMessage}",
                ex.Message);
            throw new ArgumentException("CSV file contains invalid data format", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "There was an error try to read CSV file - ExceptionMessage: {ExceptionMessage}",
                ex.Message);
            throw new ArgumentException("Error reading CSV file", ex);
        }
    }
}
