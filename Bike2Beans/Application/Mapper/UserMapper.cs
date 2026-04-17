

using Bike2Beans.Application.DTOs;
using Bike2Beans.Application.Mapping;
using Bike2Beans.Domain.Entities;

namespace Bike2Beans.Domain.Mapper;

public class UserMapper : BaseMapper<User, UserDto>
{
    public override UserDto ToDto(User entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        return new UserDto
        (
            entity.Id,
            entity.Email,
            entity.FirstName,
            entity.LastName

        );
    }
    public override User ToEntity(UserDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        return new User
        (
            dto.Id,
            dto.Email,
            dto.FirstName,
            dto.LastName
        );
    }

    public override IEnumerable<UserDto> ToDto(IEnumerable<User> entities)
    {
        if (entities == null) return Enumerable.Empty<UserDto>();
        return entities.Select(ToDto);
    }

    public override IEnumerable<User> ToEntity(IEnumerable<UserDto> dtos)
    {
        if (dtos == null) return Enumerable.Empty<User>();
        return dtos.Select(ToEntity);
    }
}
