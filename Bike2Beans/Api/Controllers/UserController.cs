using Bike2Beans.Application.CommandsAndQueries.UserCnQ;
using Bike2Beans.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bike2Beans.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{

    private readonly IMediator _mediator;

    public UserController(IMediator sender) => _mediator = sender;

    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromRoute] User user, CancellationToken ct)
    {
        var createCommand = new CreateUserCommand(
            user.AuthId,
            user.Email,
            user.FirstName,
            user.LastName
        );
        var createdUser = await _mediator.Send(createCommand, ct);
        return Ok(createdUser);
    }
    [HttpPost("delete")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id, [FromRoute] Guid authId, CancellationToken ct)
    {
        var deleteCommand = new DeleteUserCommand(id, authId);
        await _mediator.Send(deleteCommand, ct);
        return NoContent();
    }

    [HttpPost("patch")]
    public async Task<IActionResult> PatchUser([FromRoute] User user, CancellationToken ct)
    {
        var patchCommand = new PatchUserCommand(user);
        var patchedUser = await _mediator.Send(patchCommand, ct);
        return Ok(patchedUser);
    }
    [HttpPost("update")]
    public async Task<IActionResult> UpdateUser([FromRoute] User user, CancellationToken ct)
    {
        var updateCommand = new UpdateUserCommand(user);
        var updatedUser = await _mediator.Send(updateCommand, ct);
        return Ok(updatedUser);
    }
}