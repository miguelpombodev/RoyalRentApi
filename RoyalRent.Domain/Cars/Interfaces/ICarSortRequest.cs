namespace RoyalRent.Domain.Abstractions.Filters;

public enum CarSortBy
{
    Name,
    Model,
    Year,
    Price,
    Make,
    Type,
    Seats
}

public enum SortType
{
    Asc,
    Desc
}

public interface ICarSortRequest
{
    CarSortBy SortBy { get; set; }
    SortType SortType { get; set; }
}
