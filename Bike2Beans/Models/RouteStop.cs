namespace Bike2Beans.Models;

public class RouteStop
{
    public required string PlaceId { get; set; }
    public required string Name { get; set; }
    public double Lat { get; set; }
    public double Lng { get; set; }
}