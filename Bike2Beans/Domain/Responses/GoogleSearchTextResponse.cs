using Bike2Beans.Domain.Interfaces;

namespace Bike2Beans.Infrastructure.Responses;

public sealed class GoogleSearchTextResponse : ILocationPaginatedResponse
{
    public string? NextPageToken { get; }

    public List<ILocation> Locations { get; }
}
public sealed class GooglePlace : ILocation
{
    public string? Id { get; }
    public GoogleDisplayName? DisplayName { get; }
    public string? FormattedAddress { get; }
    public double? Rating { get; }
    public int? UserRatingCount { get; }
    public GoogleLocation? Location { get; }
    public string Name => DisplayName.Text;
    public double Latitude => Location.Latitude;
    public double Longitude => Location.Longitude;
}
public sealed class GoogleDisplayName { public string? Text { get; set; } }

public sealed class GoogleLocation { public double Latitude { get; set; } public double Longitude { get; set; } }
