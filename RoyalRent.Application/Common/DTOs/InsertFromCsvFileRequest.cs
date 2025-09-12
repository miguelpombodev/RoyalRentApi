using Microsoft.AspNetCore.Http;

namespace RoyalRent.Application.DTOs.Inputs;

public record InsertFromCsvFileRequest(IFormFile File);
