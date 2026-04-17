
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Bike2Beans.Domain.Entities;



public class RouteStop
{
    public RouteStop(Guid id, string placeId, string name, string address, double lat, double lng, LocationTypeEnum locationType)
    {
        this.Id = id;
        this.PlaceId = placeId;
        this.Name = name;
        this.Address = address;
        this.Lat = lat;
        this.Lng = lng;
        this.LocationType = locationType;
    }
    [BsonId(IdGenerator = typeof(GuidGenerator))]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }

    [BsonElement("placeId")]
    public string PlaceId { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("address")]
    public string Address { get; set; }


    [BsonElement("lat")]
    public double Lat { get; set; }
    [BsonElement("lng")]
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
