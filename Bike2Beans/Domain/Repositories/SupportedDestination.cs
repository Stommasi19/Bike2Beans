namespace Bike2Beans.Domain.Repositories;

public static class DestinationTypes
{
    public static readonly IReadOnlyList<string> IncludedTypes =
        new List<string>
        {
            "cafe",
            "bakery"
        };
}
