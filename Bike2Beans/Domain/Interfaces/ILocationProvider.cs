
using Google.Maps.Places.V1;
using Bike2Beans.Infrastructure;

namespace Bike2Beans.Domain.Interfaces;

public interface ILocationProvider
{
    Task<ILocationPaginatedResponse> SearchPlacesByTextAsync(
        string text,
        int pageSize,
        string? pageToken = null,
        CancellationToken ct = default
    );
}
