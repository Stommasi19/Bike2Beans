namespace Bike2Beans.Domain.DTOs;

public record PagedResult<T>(
    List<T> Items,
    string? NextPageToken
);