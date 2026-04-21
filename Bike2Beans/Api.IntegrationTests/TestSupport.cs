using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.IntegrationTests;

internal static class ControllerTestSupport
{
    public static TController WithClaims<TController>(TController controller, params Claim[] claims)
        where TController : ControllerBase
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"))
            }
        };

        return controller;
    }
}

internal sealed class RecordingMediator : IMediator
{
    private readonly Dictionary<Type, Func<object, CancellationToken, object?>> _handlers = [];

    public object? LastRequest { get; private set; }
    public CancellationToken LastCancellationToken { get; private set; }

    public void Register<TRequest, TResponse>(Func<TRequest, CancellationToken, TResponse> handler)
        where TRequest : IRequest<TResponse>
    {
        _handlers[typeof(TRequest)] = (request, cancellationToken) =>
            handler((TRequest)request, cancellationToken);
    }

    public Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default
    )
    {
        LastRequest = request;
        LastCancellationToken = cancellationToken;

        if (!_handlers.TryGetValue(request.GetType(), out var handler))
        {
            throw new InvalidOperationException(
                $"No mediator handler was registered for {request.GetType().Name}."
            );
        }

        return Task.FromResult((TResponse)handler(request, cancellationToken)!);
    }

    public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest
    {
        LastRequest = request;
        LastCancellationToken = cancellationToken;

        if (_handlers.TryGetValue(request.GetType(), out var handler))
        {
            _ = handler(request, cancellationToken);
        }

        return Task.CompletedTask;
    }

    public Task<object?> Send(object request, CancellationToken cancellationToken = default)
    {
        LastRequest = request;
        LastCancellationToken = cancellationToken;

        if (_handlers.TryGetValue(request.GetType(), out var handler))
        {
            return Task.FromResult(handler(request, cancellationToken));
        }

        throw new InvalidOperationException(
            $"No mediator handler was registered for {request.GetType().Name}."
        );
    }

    public Task Publish(object notification, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;

    public Task Publish<TNotification>(
        TNotification notification,
        CancellationToken cancellationToken = default
    )
        where TNotification : INotification => Task.CompletedTask;

    public IAsyncEnumerable<TResponse> CreateStream<TResponse>(
        IStreamRequest<TResponse> request,
        CancellationToken cancellationToken = default
    ) => EmptyAsync<TResponse>();

    public IAsyncEnumerable<object?> CreateStream(
        object request,
        CancellationToken cancellationToken = default
    ) => EmptyAsync<object?>();

    private static async IAsyncEnumerable<T> EmptyAsync<T>()
    {
        await Task.CompletedTask;
        yield break;
    }
}
