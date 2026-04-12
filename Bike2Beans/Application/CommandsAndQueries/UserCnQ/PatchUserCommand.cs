

using Bike2Beans.Application.DTOs;
using Bike2Beans.Application.Interfaces;
using Bike2Beans.Domain.Entities;
using Bike2Beans.Domain.Mapper;
using MediatR;

namespace Bike2Beans.Application.CommandsAndQueries.UserCnQ;

public record PatchUserCommand
(
    User User

    ) : IRequest<UserDto>;


public class PatchUserHandler : IRequestHandler<PatchUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper<User, UserDto> _mapper;

    public PatchUserHandler(IUserRepository userRepository, IMapper<User, UserDto> mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    public async Task<UserDto> Handle(PatchUserCommand request, CancellationToken cancellationToken)
    {

        var patchedUser = await _userRepository.PatchUserAsync(request.User, cancellationToken);
        return _mapper.ToDto(patchedUser);
    }
}