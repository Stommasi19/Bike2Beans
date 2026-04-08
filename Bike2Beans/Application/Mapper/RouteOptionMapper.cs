
using Bike2Beans.Application.DTOs;
using Bike2Beans.Application.Interfaces;
using Bike2Beans.Application.Mapping;
using Bike2Beans.Domain.Entities;
using Bike2Beans.Domain.Mapper;

namespace Bike2Beans.Application.Mapper;

public class RouteOptionMapper : BaseMapper<RouteOption, RouteOptionDto>
{
    private readonly IMapper<RouteStop, RouteStopDto> _mapper;
    public RouteOptionMapper(IMapper<RouteStop, RouteStopDto> mapper)
    {
        _mapper = mapper;
    }
    public override RouteOptionDto ToDto(RouteOption entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        return new RouteOptionDto(
            entity.Id,
            entity.OptionIndex,
            entity.DistanceMeters,
            entity.DurationSeconds,
            entity.GeometryType,
            entity.Coordinates
            );
    }

    public override RouteOption ToEntity(RouteOptionDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        return new RouteOption(
            dto.OptionIndex,
            dto.DistanceMeters,
            dto.DurationSeconds,
            dto.GeometryType,
            dto.Coordinates
        );
    }
    public override IEnumerable<RouteOptionDto> ToDto(IEnumerable<RouteOption> entities)
    {
        if (entities == null) return Enumerable.Empty<RouteOptionDto>();
        return entities.Select(ToDto);
    }
    public override IEnumerable<RouteOption> ToEntity(IEnumerable<RouteOptionDto> dtos)
    {
        if (dtos == null) return Enumerable.Empty<RouteOption>();
        return dtos.Select(ToEntity);
    }
}
