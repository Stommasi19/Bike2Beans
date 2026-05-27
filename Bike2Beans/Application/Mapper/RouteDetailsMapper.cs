
using Bike2Beans.Application.DTOs;
using Bike2Beans.Application.Interfaces;
using Bike2Beans.Application.Mapping;
using Bike2Beans.Domain.Entities;
using Bike2Beans.Domain.Mapper;

namespace Bike2Beans.Application.Mapper;

public class RouteDetailsMapper : BaseMapper<RouteDetails, RouteDetailsDto>
{
    private readonly IMapper<RouteStop, RouteStopDto> _mapper;
    public RouteDetailsMapper(IMapper<RouteStop, RouteStopDto> mapper)
    {
        _mapper = mapper;
    }
    public override RouteDetailsDto ToDto(RouteDetails entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        return new RouteDetailsDto(
            entity.Id,
            entity.Name ?? string.Empty,
            entity.StartLocation,
            entity.EndLocation,
            (entity.RouteStops ?? []).Select(_mapper.ToDto).ToList(),
            entity.Mileage ?? 0
        );
    }

    public override RouteDetails ToEntity(RouteDetailsDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        return new RouteDetails(
            dto.Name,
            dto.StartLocation,
            dto.EndLocation,
            (dto.RouteStops ?? []).Select(_mapper.ToEntity).ToList(),
            dto.Mileage
        )
        {
            Id = dto.Id
        };
    }
    public override IEnumerable<RouteDetailsDto> ToDto(IEnumerable<RouteDetails> entities)
    {
        if (entities == null) return Enumerable.Empty<RouteDetailsDto>();
        return entities.Select(ToDto);
    }
    public override IEnumerable<RouteDetails> ToEntity(IEnumerable<RouteDetailsDto> dtos)
    {
        if (dtos == null) return Enumerable.Empty<RouteDetails>();
        return dtos.Select(ToEntity);
    }
}
