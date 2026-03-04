
using Google.Maps.Places.V1;
using Bike2Beans.Infrastructure;
using Bike2Beans.Domain.Entities;
namespace Bike2Beans.Domain.Interfaces;

public interface ILocationProvider
{
    Task<LocationPaginatedResponse> SearchPlacesByTextAsync(
        string text,
        int pageSize,
        string? pageToken = null,
        CancellationToken ct = default
    );
}
