using Microsoft.AspNetCore.Http;

namespace RoyalRent.Presentation.Cars.Requests;

public record InsertFromCsvFileRequest(IFormFile File);
