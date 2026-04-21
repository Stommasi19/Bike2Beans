using Bike2Beans.Application.DTOs;
using Bike2Beans.Application.Interfaces;
using Bike2Beans.Domain.Entities;

namespace Application.Tests;

internal sealed class RecordingCoffeeshopRepository : ICoffeeshopRepository
{
    public List<Coffeeshop> GetAllResult { get; set; } = [];
    public Coffeeshop? InsertedShop { get; private set; }
    public CancellationToken InsertCancellationToken { get; private set; }
    public Func<Coffeeshop, CancellationToken, Task<Coffeeshop>> InsertAsyncHandler { get; set; } =
        (shop, _) => Task.FromResult(shop);

    public Task<List<Coffeeshop>> GetAllAsync() => Task.FromResult(GetAllResult);

    public Task<Coffeeshop> InsertAsync(Coffeeshop shop, CancellationToken ct = default)
    {
        InsertedShop = shop;
        InsertCancellationToken = ct;
        return InsertAsyncHandler(shop, ct);
    }
}

internal sealed class RecordingRouteProvider : IRouteProvider
{
    public List<double>? CapturedStartLocation { get; private set; }
    public List<double>? CapturedEndLocation { get; private set; }
    public List<RouteStopDto>? CapturedStops { get; private set; }
    public CancellationToken CapturedCancellationToken { get; private set; }
    public List<RouteOptionDto> Result { get; set; } = [];

    public Task<List<RouteOptionDto>> CreateRoute(
        List<double> startLocation,
        List<double>? endLocation,
        List<RouteStopDto> stops,
        CancellationToken ct = default
    )
    {
        CapturedStartLocation = startLocation;
        CapturedEndLocation = endLocation;
        CapturedStops = stops;
        CapturedCancellationToken = ct;

        return Task.FromResult(Result);
    }
}

internal sealed class RecordingUserRepository : IUserRepository
{
    public User? GetByIdResult { get; set; }
    public User? GetByEmailResult { get; set; }
    public string? RequestedUserId { get; private set; }
    public User? CreatedUser { get; private set; }
    public User? UpdatedUser { get; private set; }
    public (string Id, string Email, string? FirstName, string? LastName)? PatchRequest { get; private set; }
    public string? DeletedUserId { get; private set; }
    public CancellationToken LastCancellationToken { get; private set; }

    public Func<User, CancellationToken, Task<User>> CreateUserAsyncHandler { get; set; } =
        (user, _) => Task.FromResult(user);

    public Func<User, CancellationToken, Task<User>> UpdateUserAsyncHandler { get; set; } =
        (user, _) => Task.FromResult(user);

    public Func<string, string, string?, string?, CancellationToken, Task<User>> PatchUserAsyncHandler { get; set; } =
        (id, email, firstName, lastName, _) => Task.FromResult(
            new User(id, email, firstName ?? string.Empty, lastName ?? string.Empty)
        );

    public Task<User> CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        CreatedUser = user;
        LastCancellationToken = cancellationToken;
        return CreateUserAsyncHandler(user, cancellationToken);
    }

    public Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        RequestedUserId = id;
        LastCancellationToken = cancellationToken;
        return Task.FromResult(GetByIdResult);
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult(GetByEmailResult);
    }

    public Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        UpdatedUser = user;
        LastCancellationToken = cancellationToken;
        return UpdateUserAsyncHandler(user, cancellationToken);
    }

    public Task<User> PatchUserAsync(
        string id,
        string email,
        string? firstName,
        string? lastName,
        CancellationToken cancellationToken = default
    )
    {
        PatchRequest = (id, email, firstName, lastName);
        LastCancellationToken = cancellationToken;
        return PatchUserAsyncHandler(id, email, firstName, lastName, cancellationToken);
    }

    public Task DeleteUserAsync(string id, CancellationToken cancellationToken = default)
    {
        DeletedUserId = id;
        LastCancellationToken = cancellationToken;
        return Task.CompletedTask;
    }
}

internal sealed class RecordingUserBootstrapRepository : IUserBootstrapRepository
{
    public User? GetByIdResult { get; set; }
    public string? RequestedUserId { get; private set; }
    public CancellationToken LastCancellationToken { get; private set; }

    public Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        RequestedUserId = id;
        LastCancellationToken = cancellationToken;
        return Task.FromResult(GetByIdResult);
    }

    public Task<User?> GetByEmailAsync(string id, CancellationToken cancellationToken = default)
    {
        LastCancellationToken = cancellationToken;
        return Task.FromResult<User?>(null);
    }
}
