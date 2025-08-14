namespace RoyalRent.Domain.Abstractions.Filters;

public interface IGetAllAvailableCarsFilters
{
    bool IsFeatured { get; set; }
}
