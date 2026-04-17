
using Bike2Beans.Application.DTOs;
using Bike2Beans.Application.Interfaces;
using Bike2Beans.Domain.Entities;
using MediatR;

namespace Bike2Beans.Application.CommandsAndQueries.UserCnQ;

public record GetUserCommand(string UserId) : IRequest<UserDto>;


public class GetUserHandler : IRequestHandler<GetUserCommand, UserDto>
{
    private readonly IUserBootstrapRepository _userBootstrapRepository;
    private readonly IMapper<User, UserDto> _userMapper;

    public GetUserHandler(IUserBootstrapRepository userBootstrapRepository, IMapper<User, UserDto> userMapper)
    {
        _userBootstrapRepository = userBootstrapRepository;
        _userMapper = userMapper;
    }

    public async Task<UserDto> Handle(GetUserCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.UserId))
        {
            throw new ArgumentException("User ID is required.");
        }

        var user = await _userBootstrapRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (user == null)
        {
            throw new Exception($"User with ID {command.UserId} not found.");
        }

        return _userMapper.ToDto(user);
    }
}
