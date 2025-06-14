namespace RoyalRent.Application.Cars.Model;

public class CarsCsv
{
    public CarsCsv(string imageUrl, string name, string model, string make, int year, string type, string color)
    {
        ImageUrl = imageUrl;
        Name = name;
        Model = model;
        Make = make;
        Year = year;
        Type = type;
        Color = color;
    }

    public string ImageUrl { get; set; }
    public string Name { get; set; }
    public string Model { get; set; }
    public string Make { get; set; }
    public int Year { get; set; }
    public string Type { get; set; }
    public string Color { get; set; }
}
