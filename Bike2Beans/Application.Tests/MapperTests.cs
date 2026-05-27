using Bike2Beans.Application.DTOs;
using Bike2Beans.Application.Mapper;
using Bike2Beans.Domain.Entities;
using Bike2Beans.Domain.Mapper;

namespace Application.Tests;

public class MapperTests
{
    private readonly RouteDetailsMapper _routeDetailsMapper = new(new RouteStopMapper());

    [Fact]
    public void RouteDetailsMapper_ToDto_DefaultsNullNameAndStops()
    {
        var route = new RouteDetails(
            name: string.Empty,
            startLocation: [47.6, -122.3],
            endLocation: null,
            routeStops: null,
            mileage: 12.5
        )
        {
            Id = "route-1",
            Name = null
        };

        var dto = _routeDetailsMapper.ToDto(route);

        Assert.Equal(string.Empty, dto.Name);
        Assert.NotNull(dto.RouteStops);
        Assert.Empty(dto.RouteStops);
    }

    [Fact]
    public void RouteDetailsMapper_ToEntity_DefaultsNullStops()
    {
        var dto = new RouteDetailsDto(
            "route-1",
            "Morning loop",
            [47.6, -122.3],
            null,
            null,
            12.5
        );

        var route = _routeDetailsMapper.ToEntity(dto);

        Assert.NotNull(route.RouteStops);
        Assert.Empty(route.RouteStops);
    }

    [Fact]
    public void RouteStopMapper_ToEntity_DefaultsNullAddress()
    {
        var mapper = new RouteStopMapper();
        var dto = new RouteStopDto(
            Guid.NewGuid(),
            "place-1",
            "No Address Cafe",
            null,
            LocationTypeEnum.Coffeeshop,
            47.6,
            -122.3
        );

        var entity = mapper.ToEntity(dto);

        Assert.Equal(string.Empty, entity.Address);
    }
}
