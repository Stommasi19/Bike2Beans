using Bike2Beans.Domain.DTOs;
using Bike2Beans.Domain.Interfaces;

namespace Bike2Beans.Infrastructure.Responses;

public sealed class GoogleSearchTextResponse
{
    public string? NextPageToken { get; set; }

    public List<GooglePlace> Places { get; set; }
}
public sealed class GooglePlace
{
    public string? Id { get; }
    public GoogleDisplayName? DisplayName { get; }
    public string? FormattedAddress { get; }
    public double? Rating { get; }
    public int? UserRatingCount { get; }
    public GoogleLocation? Location { get; }

}
public sealed class GoogleDisplayName { public string? Text { get; set; } }

public sealed class GoogleLocation { public double Latitude { get; set; } public double Longitude { get; set; } }
