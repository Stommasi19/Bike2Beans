

using Bike2Beans.Application.DTOs;
using Bike2Beans.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bike2Beans.Application.CommandsAndQueries.UserCnQ;


public record DeleteUserCommand
(
    string Id
    ) : IRequest<IActionResult>;


public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, IActionResult>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IActionResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.DeleteUserAsync(request.Id, cancellationToken);
        return new NoContentResult();
    }

}
