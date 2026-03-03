namespace Bike2Beans.Domain.Interfaces;


public interface ILocation
{
    public string? Id { get; }

    string Name { get; }
    public string? FormattedAddress { get; }

    double Latitude { get; }
    double Longitude { get; }
    public double? Rating { get; }
    public int? UserRatingCount { get; }
}
