using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;


namespace Bike2Beans.Domain.Entities;

public class Coffeeshop
{
    [BsonId(IdGenerator = typeof(GuidGenerator))]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]   
     public Guid Id { get; set; }

    [BsonElement("id")]
    public string? PlaceId { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = null!;

    [BsonElement("address")]
    public string? Address { get; set; }

    [BsonElement("lat")]
    public double? Lat { get; set; }


    [BsonElement("lng")]
    public double? Lng { get; set; }


    [BsonElement("rating")]
    public double? Rating { get; set; }

    [BsonElement("userRatingsTotal")]
    public int? UserRatingsTotal { get; set; }

    public Coffeeshop(string placeId, string name, string? address, double? lat, double? lng, double? rating, int? userRatingsTotal)
    {

        this.PlaceId = placeId;
        this.Name = name;
        this.Address = address;
        this.Lat = lat;
        this.Lng = lng;
        this.Rating = rating;
        this.UserRatingsTotal = userRatingsTotal;

    }
}
