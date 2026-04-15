
namespace Bike2Beans.Application.CommandsAndQueries.Types;

public record DestinationTypes
{
    public static IReadOnlyList<string> IncludedTypes =
        new List<string>
        {
            "cafe",
            "bakery"
        };
}