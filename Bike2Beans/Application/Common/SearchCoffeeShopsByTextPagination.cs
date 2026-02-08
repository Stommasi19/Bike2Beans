namespace Bike2Beans.Application.Common;

public record PagedResult<T>(
    List<T> Items,
    string? NextPageToken
);