using Bike2Beans.Application.CommandsAndQueries.UserCnQ;
using Bike2Beans.Domain.Entities;
using Bike2Beans.Domain.Mapper;

namespace Application.Tests;

public class UserHandlerTests
{
    [Fact]
    public async Task CreateUserHandler_CreatesUser_WhenRepositoryDoesNotHaveOne()
    {
        var repository = new RecordingUserRepository();
        var createdUser = new User("firebase-1", "rider@example.com", "Rider", "One");
        repository.CreateUserAsyncHandler = (_, _) => Task.FromResult(createdUser);
        var handler = new CreateUserCommandHandler(repository, new UserMapper());
        var command = new CreateUserCommand("firebase-1", "rider@example.com", "Rider", "One");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(repository.CreatedUser);
        Assert.Equal(command.UserId, repository.CreatedUser.Id);
        Assert.Equal(command.Email, repository.CreatedUser.Email);
        Assert.Equal(command.FirstName, repository.CreatedUser.FirstName);
        Assert.Equal(command.LastName, repository.CreatedUser.LastName);
        Assert.Null(repository.UpdatedUser);

        Assert.Equal(createdUser.Id, result.Id);
        Assert.Equal(createdUser.Email, result.Email);
    }

    [Fact]
    public async Task CreateUserHandler_UpdatesExistingUser_WhenRepositoryAlreadyHasOne()
    {
        var existingUser = new User("firebase-2", "old@example.com", "Old", "Name");
        var updatedUser = new User("firebase-2", "new@example.com", "New", "Name");
        var repository = new RecordingUserRepository
        {
            GetByIdResult = existingUser,
            UpdateUserAsyncHandler = (_, _) => Task.FromResult(updatedUser)
        };
        var handler = new CreateUserCommandHandler(repository, new UserMapper());
        var command = new CreateUserCommand("firebase-2", "new@example.com", "New", "Name");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(command.UserId, repository.RequestedUserId);
        Assert.Null(repository.CreatedUser);
        Assert.NotNull(repository.UpdatedUser);
        Assert.Same(existingUser, repository.UpdatedUser);
        Assert.Equal(command.Email, repository.UpdatedUser.Email);
        Assert.Equal(command.FirstName, repository.UpdatedUser.FirstName);
        Assert.Equal(command.LastName, repository.UpdatedUser.LastName);

        Assert.Equal(updatedUser.Id, result.Id);
        Assert.Equal(updatedUser.Email, result.Email);
    }

    [Fact]
    public async Task GetUserHandler_ThrowsArgumentException_WhenUserIdIsMissing()
    {
        var handler = new GetUserHandler(new RecordingUserBootstrapRepository(), new UserMapper());

        var error = await Assert.ThrowsAsync<ArgumentException>(
            () => handler.Handle(new GetUserCommand(""), CancellationToken.None)
        );

        Assert.Equal("User ID is required.", error.Message);
    }

    [Fact]
    public async Task GetUserHandler_Throws_WhenUserCannotBeFound()
    {
        var repository = new RecordingUserBootstrapRepository();
        var handler = new GetUserHandler(repository, new UserMapper());

        var error = await Assert.ThrowsAsync<Exception>(
            () => handler.Handle(new GetUserCommand("missing-user"), CancellationToken.None)
        );

        Assert.Equal("User with ID missing-user not found.", error.Message);
        Assert.Equal("missing-user", repository.RequestedUserId);
    }

    [Fact]
    public async Task GetUserHandler_MapsBootstrapRepositoryUser()
    {
        var repository = new RecordingUserBootstrapRepository
        {
            GetByIdResult = new User("firebase-3", "bean@example.com", "Bean", "Runner")
        };
        var handler = new GetUserHandler(repository, new UserMapper());

        var result = await handler.Handle(new GetUserCommand("firebase-3"), CancellationToken.None);

        Assert.Equal("firebase-3", result.Id);
        Assert.Equal("bean@example.com", result.Email);
        Assert.Equal("Bean", result.FirstName);
        Assert.Equal("Runner", result.LastName);
    }

    [Fact]
    public async Task PatchUserHandler_ForwardsOnlyProvidedFields()
    {
        var repository = new RecordingUserRepository
        {
            PatchUserAsyncHandler = (id, email, firstName, lastName, _) =>
                Task.FromResult(new User(id, email, firstName ?? "Existing", lastName ?? "Name"))
        };
        var handler = new PatchUserHandler(repository, new UserMapper());
        var command = new PatchUserCommand("firebase-9", "verified@example.com", "Pat", null);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(("firebase-9", "verified@example.com", "Pat", (string?)null), repository.PatchRequest);
        Assert.Equal("firebase-9", result.Id);
        Assert.Equal("verified@example.com", result.Email);
        Assert.Equal("Pat", result.FirstName);
        Assert.Equal("Name", result.LastName);
    }
}
