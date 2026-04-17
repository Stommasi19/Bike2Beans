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
    public sealed record CreateUserRequest(string FirstName, string LastName, string Email);

    private readonly IMediator _mediator;

    public UserController(IMediator sender) => _mediator = sender;

    [HttpPost("create")]
    [Authorize]

    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken ct)
    {
        var firebaseUid = GetAuthenticatedUserId();
        if (string.IsNullOrWhiteSpace(firebaseUid)) return Unauthorized();

        var createCommand = new CreateUserCommand(
            firebaseUid,
            request.Email,
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
    public async Task<IActionResult> PatchUser([FromBody] User user, CancellationToken ct)
    {
        var firebaseUid = GetAuthenticatedUserId();
        if (string.IsNullOrWhiteSpace(firebaseUid)) return Unauthorized();
        user.Id = firebaseUid;

        var patchCommand = new PatchUserCommand(user);
        var patchedUser = await _mediator.Send(patchCommand, ct);
        return Ok(patchedUser);
    }

    [HttpPut("update")]
    [Authorize]
    public async Task<IActionResult> UpdateUser([FromBody] User user, CancellationToken ct)
    {
        var firebaseUid = GetAuthenticatedUserId();
        if (string.IsNullOrWhiteSpace(firebaseUid)) return Unauthorized();
        user.Id = firebaseUid;

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
}
