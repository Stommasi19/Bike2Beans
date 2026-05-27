using System.Net;
using System.Text;
using Bike2Beans.Infrastructure.Gateways;

namespace Api.IntegrationTests;

public class GooglePlacesRestGatewayTests
{
    [Fact]
    public async Task SearchPlacesByTextAsync_ReturnsEmptyLocations_WhenPlacesIsMissing()
    {
        var handler = new StubHttpMessageHandler(
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"nextPageToken":"next"}""", Encoding.UTF8, "application/json")
            }
        );
        var gateway = new GooglePlacesRestGateway(new HttpClient(handler));

        var result = await gateway.SearchPlacesByTextAsync("coffee", 5);

        Assert.Equal("next", result.NextPageToken);
        Assert.Empty(result.Locations);
    }

    private sealed class StubHttpMessageHandler(HttpResponseMessage response) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        ) => Task.FromResult(response);
    }
}
