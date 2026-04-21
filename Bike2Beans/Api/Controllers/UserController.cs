using System.Security.Claims;
using Bike2Beans.Application.CommandsAndQueries.UserCnQ;
using Bike2Beans.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bike2Beans.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    public sealed record CreateUserRequest(string FirstName, string LastName);
    public sealed record UpdateUserRequest(string FirstName, string LastName);
    public sealed record PatchUserRequest(string? FirstName, string? LastName);

    private readonly IMediator _mediator;

    public UserController(IMediator sender) => _mediator = sender;

    [HttpPost("create")]
    [Authorize]

    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken ct)
    {
        var firebaseUid = GetAuthenticatedUserId();
        var email = GetAuthenticatedUserEmail();
        if (string.IsNullOrWhiteSpace(firebaseUid) || string.IsNullOrWhiteSpace(email)) return Unauthorized();

        var createCommand = new CreateUserCommand(
            firebaseUid,
            email,
            request.FirstName,
            request.LastName
        );
        var createdUser = await _mediator.Send(createCommand, ct);
        return Ok(createdUser);
    }

    [HttpDelete("delete")]
    [Authorize]
    public async Task<IActionResult> DeleteUser(CancellationToken ct)
    {
        var firebaseUid = GetAuthenticatedUserId();
        if (string.IsNullOrWhiteSpace(firebaseUid)) return Unauthorized();

        var deleteCommand = new DeleteUserCommand(firebaseUid);
        await _mediator.Send(deleteCommand, ct);
        return NoContent();
    }

    [HttpPatch("update")]
    [Authorize]
    public async Task<IActionResult> PatchUser([FromBody] PatchUserRequest request, CancellationToken ct)
    {
        var firebaseUid = GetAuthenticatedUserId();
        var email = GetAuthenticatedUserEmail();
        if (string.IsNullOrWhiteSpace(firebaseUid) || string.IsNullOrWhiteSpace(email)) return Unauthorized();

        var patchCommand = new PatchUserCommand(firebaseUid, email, request.FirstName, request.LastName);
        var patchedUser = await _mediator.Send(patchCommand, ct);
        return Ok(patchedUser);
    }

    [HttpPut("update")]
    [Authorize]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request, CancellationToken ct)
    {
        var firebaseUid = GetAuthenticatedUserId();
        var email = GetAuthenticatedUserEmail();
        if (string.IsNullOrWhiteSpace(firebaseUid) || string.IsNullOrWhiteSpace(email)) return Unauthorized();
        var user = new User(firebaseUid, email, request.FirstName, request.LastName);

        var updateCommand = new UpdateUserCommand(user);
        var updatedUser = await _mediator.Send(updateCommand, ct);
        return Ok(updatedUser);
    }
    [HttpGet("get")]
    [Authorize]
    public async Task<IActionResult> GetUser(CancellationToken ct)
    {
        var firebaseUid = GetAuthenticatedUserId();
        if (string.IsNullOrWhiteSpace(firebaseUid)) return Unauthorized();

        var result = await _mediator.Send(new GetUserCommand(firebaseUid), ct);
        return Ok(result);
    }

    private string? GetAuthenticatedUserId()
    {
        return User.FindFirst("user_id")?.Value ?? User.FindFirst("sub")?.Value;
    }

    private string? GetAuthenticatedUserEmail()
    {
        return User.FindFirst(ClaimTypes.Email)?.Value ?? User.FindFirst("email")?.Value;
    }
}
