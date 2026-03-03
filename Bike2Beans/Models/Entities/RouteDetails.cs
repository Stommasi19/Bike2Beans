using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Bike2Beans.Dtos;


namespace Bike2Beans.Models.Entities;

public class RouteDetails
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string? Name { get; set; }
    [BsonElement("startLocation")]
    public List<double> StartLocation { get; set; } = new();

    [BsonElement("endLocation")]
    public List<double>? EndLocation { get; set; } = new();



    [BsonElement("routeStops")]
    public List<CoffeeShopDto>? Stops { get; set; } = new();

    [BsonElement("mileage")]
    public double? Mileage { get; set; }

}