
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bike2Beans.Domain.Entities;



public class RouteStop
{
    public RouteStop(string Id, string PlaceId, string name, string address, double lat, double lng, LocationTypeEnum locationType)
    {
        this.Id = Id;
        this.PlaceId = PlaceId;
        this.Name = name;
        this.Address = address;
        this.Lat = lat;
        this.Lng = lng;
        this.LocationType = locationType;
    }
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("PlaceId")]
    public string PlaceId { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("address")]
    public string Address { get; set; }


    [BsonElement("Lat")]
    public double Lat { get; set; }
    [BsonElement("Lng")]
    public double Lng { get; set; }


    [BsonElement("locationType")]
    public LocationTypeEnum LocationType { get; set; } = LocationTypeEnum.Other;

}
public enum LocationTypeEnum
{
    Coffeeshop,
    Landmark,
    Other
}