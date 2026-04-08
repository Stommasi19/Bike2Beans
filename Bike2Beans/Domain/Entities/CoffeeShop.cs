using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Bike2Beans.Domain.Entities;

public class Coffeeshop
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

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

    public Coffeeshop(string PlaceId, string Name, string? Address, double? Lat, double? Lng, double? Rating, int? UserRatingsTotal)
    {
        this.PlaceId = PlaceId;
        this.Name = Name;
        this.Address = Address;
        this.Lat = Lat;
        this.Lng = Lng;
        this.Rating = Rating;
        this.UserRatingsTotal = UserRatingsTotal;

    }
}
