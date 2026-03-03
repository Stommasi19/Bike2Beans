using Bike2Beans.Domain.CommandsAndQueries.CoffeeshopNonGoogle;
using Bike2Beans.Models.Entities;
using Bike2Beans.Models.Mapping;

namespace Bike2Beans.Domain.Mapper;

public class CoffeeshopMapper : BaseMapper<Coffeeshop, CoffeeshopDto>
{
    public override CoffeeshopDto ToDto(Coffeeshop entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        return new CoffeeshopDto
        (
            entity.Id,
           entity.Name,
            entity.Address,
            entity.Lat,
            entity.Lng,
            entity.Rating
        );
    }
    public override Coffeeshop ToEntity(CoffeeshopDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        return new Coffeeshop
        (
            Id = dto.Id,
            Name = dto.Name,
            Address = dto.Address,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            Rating = dto.Rating
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