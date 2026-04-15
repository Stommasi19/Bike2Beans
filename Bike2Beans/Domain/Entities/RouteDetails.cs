using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Bike2Beans.Domain.Entities;

public class RouteDetails
{


    public RouteDetails(string name, List<double> startLocation, List<double>? endLocation, List<RouteStop>? routeStops, double mileage)
    {
        this.Name = name;
        this.StartLocation = startLocation;
        this.EndLocation = endLocation;
        this.RouteStops = routeStops;
        this.Mileage = mileage;
    }


    [BsonId(IdGenerator = typeof(GuidGenerator))]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }

    [BsonElement("name")]
    public string? Name { get; set; }

    [BsonElement("startLocation")]
    public List<double> StartLocation { get; set; } = new();

    [BsonElement("endLocation")]
    public List<double>? EndLocation { get; set; } = new();


    [BsonElement("routeStops")]
    public List<RouteStop>? RouteStops { get; set; } = new();

    [BsonElement("mileage")]
    public double? Mileage { get; set; }

}