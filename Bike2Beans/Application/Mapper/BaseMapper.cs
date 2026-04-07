using Bike2Beans.Application.Interfaces;

namespace Bike2Beans.Application.Mapping;


public abstract class BaseMapper<TEntity, TDto> : IMapper<TEntity, TDto>
{
    public abstract TDto ToDto(TEntity entity);

    public abstract TEntity ToEntity(TDto dto);

    public virtual IEnumerable<TDto> ToDto(IEnumerable<TEntity> entities)
    {
        if (entities == null) return Enumerable.Empty<TDto>();
        return entities.Select(ToDto);
    }

    public virtual IEnumerable<TEntity> ToEntity(IEnumerable<TDto> dtos)
    {
        if (dtos == null) return Enumerable.Empty<TEntity>();
        return dtos.Select(ToEntity);
    }
}
