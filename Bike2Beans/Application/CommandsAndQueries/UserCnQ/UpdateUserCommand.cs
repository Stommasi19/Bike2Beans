

using Bike2Beans.Application.DTOs;
using Bike2Beans.Application.Interfaces;
using Bike2Beans.Domain.Entities;
using Bike2Beans.Domain.Mapper;
using MediatR;

namespace Bike2Beans.Application.CommandsAndQueries.UserCnQ;

public record UpdateUserCommand
(
    User User

    ) : IRequest<UserDto>;


public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper<User, UserDto> _mapper;

    public UpdateUserHandler(IUserRepository userRepository, IMapper<User, UserDto> mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var updatedUser = await _userRepository.UpdateUserAsync(request.User, cancellationToken);
        return _mapper.ToDto(updatedUser);
    }
}