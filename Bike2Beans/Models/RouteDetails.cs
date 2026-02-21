using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Bike2Beans.Models;

public class RouteDetails
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string? Name { get; set; }

    [BsonElement("routestops")]
    public List<RouteStop>? Stops { get; set; } = new();

    [BsonElement("mileage")]
    public double? Mileage { get; set; }

}