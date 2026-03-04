
namespace Bike2Beans.Domain.Interfaces;

public interface IRouteDetails
{
    string RouteName { get; set; }
    public ILocation StartLocation { get; set; }
    public ILocation EndLocation { get; set; }
    public List<ILocation> Stops { get; set; }

    double TotalDistanceMeters { get; set; }
}