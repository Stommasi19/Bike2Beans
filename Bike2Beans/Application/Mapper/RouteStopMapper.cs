

using Bike2Beans.Application.DTOs;
using Bike2Beans.Application.Mapping;
using Bike2Beans.Domain.Entities;

namespace Bike2Beans.Domain.Mapper;

public class RouteStopMapper : BaseMapper<RouteStop, RouteStopDto>
{
    public override RouteStopDto ToDto(RouteStop entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        return new RouteStopDto
        (
            entity.Id,
            entity.PlaceId,
            entity.Name,
            entity.Address,
            entity.LocationType,
            entity.Lat,
            entity.Lng
        );
    }
    public override RouteStop ToEntity(RouteStopDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        return new RouteStop
        (
            Id: dto.Id,
            PlaceId: dto.PlaceId,
            name: dto.Name,
            address: dto.Address,
            lat: dto.Lat,
            lng: dto.Lng,
            locationType: dto.LocationType
        );
    }

    public override IEnumerable<RouteStopDto> ToDto(IEnumerable<RouteStop> entities)
    {
        if (entities == null) return Enumerable.Empty<RouteStopDto>();
        return entities.Select(ToDto);
    }

    public override IEnumerable<RouteStop> ToEntity(IEnumerable<RouteStopDto> dtos)
    {
        if (dtos == null) return Enumerable.Empty<RouteStop>();
        return dtos.Select(ToEntity);
    }
}