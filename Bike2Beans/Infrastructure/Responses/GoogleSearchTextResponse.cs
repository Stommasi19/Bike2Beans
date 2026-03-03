namespace Bike2Beans.Infrastructure.Responses;

public sealed class GoogleSearchTextResponse
{
    public List<GooglePlace> Places { get; set; } = new();
    public string? NextPageToken { get; set; }
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
