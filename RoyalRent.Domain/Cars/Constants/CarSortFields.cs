namespace RoyalRent.Domain.Constants;

public static class CarSortFields
{
    public const string Name = "name";
    public const string Model = "model";
    public const string Year = "year";
    public const string Price = "price";
    public const string Make = "make";
    public const string Type = "type";
    public const string Seats = "seats";

    public static readonly string[] ValidFields = [Name, Model, Year, Price, Make, Type, Seats];
}

public static class SortDirections
{
    public const string Asc = "asc";
    public const string Desc = "desc";

    public static readonly string[] ValidDirections = [Asc, Desc];
}
