namespace Bike2Beans.Application.Interfaces;

public interface IMapper<TEntity, TDto>
{
    /// Maps from entity to DTO
    TDto ToDto(TEntity entity);

    /// Maps from DTO to entity
    TEntity ToEntity(TDto dto);

    /// Maps from entity collection to DTO collection
    IEnumerable<TDto> ToDto(IEnumerable<TEntity> entities);

    /// Maps from DTO collection to entity collection
    IEnumerable<TEntity> ToEntity(IEnumerable<TDto> dtos);
}
