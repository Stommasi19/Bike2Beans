using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Bike2Beans.Api.Configuration;

public sealed class ApiCostGuardOptions
{
    public const string SectionName = "ApiCostGuards";

    public int NearbyRadiusMetersMax { get; init; } = 2500;
    public int NearbyResultCountMax { get; init; } = 12;
    public int TextSearchPageSizeMax { get; init; } = 6;
    public int TextSearchMinLength { get; init; } = 2;
    public int AutocompleteMinLength { get; init; } = 2;
    public int RouteStopCountMax { get; init; } = 8;
    public int AutocompleteRequestsPerMinute { get; init; } = 30;
    public int PlaceSearchRequestsPerMinute { get; init; } = 20;
    public int RouteGenerationRequestsPerMinute { get; init; } = 10;
}

public static class ApiRateLimitPolicies
{
    public const string PlacesAutocomplete = "places-autocomplete";
    public const string PlacesSearch = "places-search";
    public const string RouteGeneration = "route-generation";

    public static string BuildPartitionKey(HttpContext httpContext, string policyName)
    {
        var userId = httpContext.User.FindFirstValue("user_id")
            ?? httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? httpContext.User.FindFirstValue("sub");

        if (!string.IsNullOrWhiteSpace(userId))
        {
            return $"{policyName}:user:{userId}";
        }

        var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return $"{policyName}:ip:{ipAddress}";
    }
}
