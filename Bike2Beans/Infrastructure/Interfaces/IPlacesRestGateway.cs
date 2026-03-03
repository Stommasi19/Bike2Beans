using Bike2Beans.Application.CoffeeShops.Queries.Search;
using Bike2Beans.Dtos;
using Google.Maps.Places.V1;
using Bike2Beans.Infrastructure;

namespace Bike2Beans.Infrastructure.Interfaces;

public interface IPlacesRestGateway
{
    Task<GoogleSearchTextResponse> SearchPlacesByTextAsync(
        SearchCoffeeShopByTextQuery query,
        CancellationToken ct = default
    );
}
