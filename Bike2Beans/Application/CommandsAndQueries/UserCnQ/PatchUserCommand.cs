

using Bike2Beans.Application.DTOs;
using Bike2Beans.Application.Interfaces;
using Bike2Beans.Domain.Entities;
using Bike2Beans.Domain.Mapper;
using MediatR;

namespace Bike2Beans.Application.CommandsAndQueries.UserCnQ;

public record PatchUserCommand
(
    string UserId,
    string Email,
    string? FirstName,
    string? LastName
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
        var patchedUser = await _userRepository.PatchUserAsync(
            request.UserId,
            request.Email,
            request.FirstName,
            request.LastName,
            cancellationToken
        );
        return _mapper.ToDto(patchedUser);
    }
}
