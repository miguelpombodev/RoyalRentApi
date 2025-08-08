namespace RoyalRent.Domain.Data.Models;

public class GetAvailableCars
{
    public GetAvailableCars(string name, string category, decimal price, int seats, string imageUrl, string transmission, string fuelType,
        string description)
    {
        Name = name;
        Category = category;
        Price = price;
        Seats = seats;
        ImageUrl = imageUrl;
        Transmission = transmission;
        FuelType = fuelType;
        Description = description;
    }

    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
    public int Seats { get; set; }
    public string ImageUrl { get; set; }
    public string Transmission { get; set; }
    public string FuelType { get; set; }
    public string Description { get; set; }
}
