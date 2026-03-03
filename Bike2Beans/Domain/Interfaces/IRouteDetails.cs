
namespace Bike2Beans.Domain.Interfaces;

public interface IRouteDetails
{
    string RouteName { get; }
    public ILocation StartLocation { get; }
    public ILocation EndLocation { get; }
    public List<ILocation> Stops { get; }

    double TotalDistanceMeters { get; }
}