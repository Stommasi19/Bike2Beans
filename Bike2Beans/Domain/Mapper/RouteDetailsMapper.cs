using Bike2Beans.Domain;

namespace Bike2Beans.Domain.Mapper;

public class RouteDetailsMapper : BaseMapper<RouteDetails, RouteDetailsDto>
{
    public override RouteDetailsDto ToDto(RouteDetails entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        return new RouteDetailsDto(
            entity.Id,
            entity.Name,
            entity.Stops.Select(s => new CoffeeShopDto(s.Id, s.Name, s.Address)).ToList(),
            entity.Mileage
        );
    }

    public override RouteDetails ToEntity(RouteDetailsDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        return new RouteDetails(
            dto.Id,
            dto.Name,
            dto.Stops.Select(s => new Coffeeshop { Id = s.Id, Name = s.Name, Address = s.Address }).ToList(),
            dto.Mileage
        );
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