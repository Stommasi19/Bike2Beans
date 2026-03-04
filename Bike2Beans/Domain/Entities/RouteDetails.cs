using Bike2Beans.Domain.DTOs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bike2Beans.Domain.Entities;

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
    public List<CoffeeshopDto>? RouteStops { get; set; } = new();

    [BsonElement("mileage")]
    public double? Mileage { get; set; }

}