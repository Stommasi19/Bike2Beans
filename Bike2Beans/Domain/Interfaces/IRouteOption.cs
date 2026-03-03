
namespace Bike2Beans.Domain.Interfaces;


public interface IRouteOption
{
    int OptionIndex { get; }
    double DistanceMeters { get; }

    double DurationSeconds { get; }

    string GeometryType { get; }
    List<List<double>> Coordinates { get; }

}