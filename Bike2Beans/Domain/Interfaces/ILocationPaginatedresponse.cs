
namespace Bike2Beans.Domain.Interfaces;

public interface ILocationPaginatedResponse
{
    public List<ILocation> Locations { get; }
    public string? NextPageToken { get; }
}