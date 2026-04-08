namespace Bike2Beans.Application.Interfaces;


public interface ILocation
{
    public string? Id { get; set; }

    string Name { get; set; }
    public string? FormattedAddress { get; set; }
    public double? Rating { get; set; }
    public int? UserRatingCount { get; set; }
    double Latitude { get; set; }
    double Longitude { get; set; }

}
