

using Bike2Beans.Application.DTOs;
using Bike2Beans.Application.Interfaces;
using Bike2Beans.Domain.Entities;
using Bike2Beans.Domain.Mapper;
using MediatR;


namespace Bike2Beans.Application.CommandsAndQueries.UserCnQ;

public record CreateUserCommand

    (
    string UserId,
    string Email,
    string FirstName,
    string LastName

    ) : IRequest<UserDto>;


public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper<User, UserDto> _mapper;

    public CreateUserCommandHandler(IUserRepository userRepository, IMapper<User, UserDto> mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (existingUser != null)
        {
            existingUser.Email = request.Email;
            existingUser.FirstName = request.FirstName;
            existingUser.LastName = request.LastName;

            var updatedUser = await _userRepository.UpdateUserAsync(existingUser, cancellationToken);
            return _mapper.ToDto(updatedUser);
        }

        var user = new User(
            request.UserId,
            request.Email,
            request.FirstName,
            request.LastName
        );
        var createdUser = await _userRepository.CreateUserAsync(user, cancellationToken);

        return _mapper.ToDto(createdUser);
    }
}
