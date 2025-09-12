using RoyalRent.Domain.Abstractions.Entities;
using RoyalRent.Domain.Entities;
using RoyalRent.Domain.Rents.Entities;

namespace RoyalRent.Domain.Cars.Entities;

public class Car : CarBaseEntity, ICarBaseEntity
{
    public Car(string name, string model, Guid carMakeId, int year, Guid carTypeId, Guid carColorId,
        string imageUrl, Guid carTransmissionsId, Guid carFuelTypeId, int seats, decimal price, string description,
        bool isFeatured) : base(name)
    {
        Model = model;
        CarMakeId = carMakeId;
        Year = year;
        CarTypeId = carTypeId;
        CarColorId = carColorId;
        ImageUrl = imageUrl;
        CarTransmissionsId = carTransmissionsId;
        Seats = seats;
        Price = price;
        CarFuelTypeId = carFuelTypeId;
        Description = description;
        IsFeatured = isFeatured;
    }

    public string Model { get; set; }
    public Guid CarMakeId { get; set; }
    public int Year { get; set; }
    public Guid CarTypeId { get; set; }
    public Guid CarColorId { get; set; }
    public string ImageUrl { get; set; }
    public Guid CarTransmissionsId { get; set; }
    public Guid CarFuelTypeId { get; set; }
    public int Seats { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public bool IsFeatured { get; set; }
    public CarMake CarMake { get; set; } = null!;
    public CarType CarType { get; set; } = null!;
    public CarColor CarColor { get; set; } = null!;
    public CarTransmissions CarTransmissions { get; set; } = null!;
    public CarFuelType CarFuelType { get; set; } = null!;
    public ICollection<Rent> Rents { get; set; } = new List<Rent>();
}
