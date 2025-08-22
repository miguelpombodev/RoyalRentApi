using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Messaging;
using RoyalRent.Application.Cars.Queries.GetAvailableCars;

namespace RoyalRent.Application.Cars.Queries.GetAvailableCarsFiltersValues;

public sealed record GetAvailableCarsFiltersValuesQuery : IQuery<Result<GetAvailableCarsFiltersValuesQueryResponse>>;
