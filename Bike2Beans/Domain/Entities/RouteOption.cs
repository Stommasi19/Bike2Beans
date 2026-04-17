using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Bike2Beans.Domain.Entities;

public class RouteOption
{


    public RouteOption(int optionIndex, double distanceMeters, double durationSeconds, string geometryType, List<List<double>> coordinates)
    {
        this.OptionIndex = optionIndex;
        this.DistanceMeters = distanceMeters;
        this.DurationSeconds = durationSeconds;
        this.GeometryType = geometryType;
        this.Coordinates = coordinates;
    }




    [BsonId(IdGenerator = typeof(GuidGenerator))]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }
    [BsonElement("optionIndex")]
    public int OptionIndex { get; set; }

    [BsonElement("distanceMeters")]
    public double DistanceMeters { get; set; }

    [BsonElement("durationSeconds")]
    public double DurationSeconds { get; set; }

    [BsonElement("geometryType")]
    public string GeometryType { get; set; }

    [BsonElement("coordinates")]
    public List<List<double>> Coordinates { get; set; }
}