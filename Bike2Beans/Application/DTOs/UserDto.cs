

namespace Bike2Beans.Application.DTOs;

public record UserDto(
    Guid Id,
    Guid AuthId,
    string Email,
    string FirstName,
    string LastName
);