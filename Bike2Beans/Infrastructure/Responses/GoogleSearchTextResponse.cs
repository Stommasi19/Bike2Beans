using Bike2Beans.Application.DTOs;
using Bike2Beans.Application.Interfaces;

namespace Bike2Beans.Infrastructure.Responses;

public sealed class GoogleSearchTextResponse
{
    public string? NextPageToken { get; set; }

    public List<GooglePlace>? Places { get; set; } = null;
}
public sealed class GooglePlace
{
    public string? Id { get; set; }
    public GoogleDisplayName? DisplayName { get; set; }
    public string? FormattedAddress { get; set; }
    public double? Rating { get; set; }
    public int? UserRatingCount { get; set; }
    public GoogleLocation? Location { get; set; }

}
public sealed class GoogleDisplayName { public string? Text { get; set; } }

public sealed class GoogleLocation { public double Latitude { get; set; } public double Longitude { get; set; } }
