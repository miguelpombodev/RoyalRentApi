using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;

namespace RoyalRent.Application.Cars.Queries.GetAvailableCars;

public sealed record GetAvailableCarsQuery(GetAllAvailableCarsFilters Filters, CarSortRequest Sort) : IQuery<Result<List<GetAvailableCarsResponse>>>;
