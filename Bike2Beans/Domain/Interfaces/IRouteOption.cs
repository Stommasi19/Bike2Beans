
namespace Bike2Beans.Domain.Interfaces;


public interface IRouteOption
{
    int OptionIndex { get; set; }
    double DistanceMeters { get; set; }

    double DurationSeconds { get; set; }

    string GeometryType { get; set; }
    List<List<double>> Coordinates { get; set; }

}