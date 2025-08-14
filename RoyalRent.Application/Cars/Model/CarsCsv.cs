using CsvHelper.Configuration.Attributes;

namespace RoyalRent.Application.Cars.Model;

public class CarsCsv
{
    public CarsCsv(string imageUrl, string name, string model, string make, int year, string type, string color,
        decimal price, int seats, string transmission, string fuelType, string description, char featured)
    {
        ImageUrl = imageUrl;
        Name = name;
        Model = model;
        Make = make;
        Year = year;
        Type = type;
        Color = color;
        Price = price;
        Seats = seats;
        Transmission = transmission;
        FuelType = fuelType;
        Description = description;
        Featured = SetFeaturedValuetoCar(featured);
    }

    [Name("imageUrl")] public string ImageUrl { get; set; }

    [Name("name")] public string Name { get; set; }

    [Name("model")] public string Model { get; set; }

    [Name("make")] public string Make { get; set; }

    [Name("year")] public int Year { get; set; }

    [Name("type")] public string Type { get; set; }

    [Name("color")] public string Color { get; set; }

    [Name("price")] public decimal Price { get; set; }

    [Name("seats")] public int Seats { get; set; }

    [Name("transmission")] public string Transmission { get; set; }

    [Name("fuel_type")] public string FuelType { get; set; }

    [Name("description")] public string Description { get; set; }
    [Name("featured")] public bool Featured { get; set; }

    private bool SetFeaturedValuetoCar(char featuredCharValue)
    {
        if (featuredCharValue.Equals('N')) return false;

        return true;
    }
}
