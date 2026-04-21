using System.Security.Claims;
using Bike2Beans.Api.Controllers;
using Bike2Beans.Application.CommandsAndQueries.UserCnQ;
using Bike2Beans.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.IntegrationTests;

public class UserControllerTests
{
    [Fact]
    public async Task CreateUser_ReturnsUnauthorized_WhenNoAuthenticatedUserIdExists()
    {
        var mediator = new RecordingMediator();
        var controller = ControllerTestSupport.WithClaims(new UserController(mediator));

        var result = await controller.CreateUser(
            new UserController.CreateUserRequest("Rider", "One"),
            CancellationToken.None
        );

        Assert.IsType<UnauthorizedResult>(result);
        Assert.Null(mediator.LastRequest);
    }

    [Fact]
    public async Task CreateUser_UsesAuthenticatedSubClaim_WhenSendingCommand()
    {
        var mediator = new RecordingMediator();
        var expected = new UserDto("firebase-123", "rider@example.com", "Rider", "One");
        mediator.Register<CreateUserCommand, UserDto>((_, _) => expected);
        var controller = ControllerTestSupport.WithClaims(
            new UserController(mediator),
            new Claim("sub", "firebase-123"),
            new Claim(ClaimTypes.Email, "rider@example.com")
        );
        using var cts = new CancellationTokenSource();

        var result = await controller.CreateUser(
            new UserController.CreateUserRequest("Rider", "One"),
            cts.Token
        );

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(expected, ok.Value);
        Assert.Equal(
            new CreateUserCommand("firebase-123", "rider@example.com", "Rider", "One"),
            Assert.IsType<CreateUserCommand>(mediator.LastRequest)
        );
        Assert.Equal(cts.Token, mediator.LastCancellationToken);
    }

    [Fact]
    public async Task PatchUser_UsesAuthenticatedIdentity_ForPartialUpdates()
    {
        var mediator = new RecordingMediator();
        var patchedUser = new UserDto("firebase-456", "patched@example.com", "Pat", "ched");
        mediator.Register<PatchUserCommand, UserDto>((_, _) => patchedUser);
        var controller = ControllerTestSupport.WithClaims(
            new UserController(mediator),
            new Claim("user_id", "firebase-456"),
            new Claim("email", "patched@example.com")
        );

        var result = await controller.PatchUser(
            new UserController.PatchUserRequest("Pat", null),
            CancellationToken.None
        );

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(patchedUser, ok.Value);
        Assert.Equal(
            new PatchUserCommand("firebase-456", "patched@example.com", "Pat", null),
            Assert.IsType<PatchUserCommand>(mediator.LastRequest)
        );
    }

    [Fact]
    public async Task DeleteUser_ReturnsNoContent_AndSendsDeleteCommand()
    {
        var mediator = new RecordingMediator();
        mediator.Register<DeleteUserCommand, IActionResult>((_, _) => new NoContentResult());
        var controller = ControllerTestSupport.WithClaims(
            new UserController(mediator),
            new Claim("sub", "firebase-789")
        );

        var result = await controller.DeleteUser(CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
        Assert.Equal(
            new DeleteUserCommand("firebase-789"),
            Assert.IsType<DeleteUserCommand>(mediator.LastRequest)
        );
    }

    [Fact]
    public async Task UpdateUser_UsesAuthenticatedEmail_InsteadOfRequestPayload()
    {
        var mediator = new RecordingMediator();
        var expected = new UserDto("firebase-222", "verified@example.com", "Route", "Runner");
        mediator.Register<UpdateUserCommand, UserDto>((_, _) => expected);
        var controller = ControllerTestSupport.WithClaims(
            new UserController(mediator),
            new Claim("sub", "firebase-222"),
            new Claim(ClaimTypes.Email, "verified@example.com")
        );

        var result = await controller.UpdateUser(
            new UserController.UpdateUserRequest("Route", "Runner"),
            CancellationToken.None
        );

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(expected, ok.Value);

        var command = Assert.IsType<UpdateUserCommand>(mediator.LastRequest);
        Assert.Equal("firebase-222", command.User.Id);
        Assert.Equal("verified@example.com", command.User.Email);
        Assert.Equal("Route", command.User.FirstName);
        Assert.Equal("Runner", command.User.LastName);
    }

    [Fact]
    public async Task GetUser_FallsBackToSubClaim_WhenUserIdClaimIsMissing()
    {
        var mediator = new RecordingMediator();
        var expected = new UserDto("firebase-101", "bean@example.com", "Bean", "Runner");
        mediator.Register<GetUserCommand, UserDto>((_, _) => expected);
        var controller = ControllerTestSupport.WithClaims(
            new UserController(mediator),
            new Claim("sub", "firebase-101")
        );

        var result = await controller.GetUser(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(expected, ok.Value);
        Assert.Equal(
            new GetUserCommand("firebase-101"),
            Assert.IsType<GetUserCommand>(mediator.LastRequest)
        );
    }
}
