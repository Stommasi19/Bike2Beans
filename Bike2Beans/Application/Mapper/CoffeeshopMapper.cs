

using Bike2Beans.Application.DTOs;
using Bike2Beans.Application.Mapping;
using Bike2Beans.Domain.Entities;

namespace Bike2Beans.Domain.Mapper;

public class CoffeeshopMapper : BaseMapper<Coffeeshop, CoffeeshopDto>
{
    public override CoffeeshopDto ToDto(Coffeeshop entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        return new CoffeeshopDto
        (
            entity.Id,
            entity.PlaceId,
            entity.Name,
            entity.Address,
            entity.Lat,
            entity.Lng,
            entity.Rating,
            entity.UserRatingsTotal

        );
    }
    public override Coffeeshop ToEntity(CoffeeshopDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        return new Coffeeshop
        (
            dto.PlaceId?.ToString() ?? string.Empty,
            dto.Name,
            dto.Address,
            dto.Lat,
            dto.Lng,
            dto.Rating,
            dto.UserRatingsTotal
        );
    }

    public override IEnumerable<CoffeeshopDto> ToDto(IEnumerable<Coffeeshop> entities)
    {
        if (entities == null) return Enumerable.Empty<CoffeeshopDto>();
        return entities.Select(ToDto);
    }

    public override IEnumerable<Coffeeshop> ToEntity(IEnumerable<CoffeeshopDto> dtos)
    {
        if (dtos == null) return Enumerable.Empty<Coffeeshop>();
        return dtos.Select(ToEntity);
    }
}