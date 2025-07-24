using ErrorOr;
using FluentAssertions;
using NSubstitute;
using DeveloperStore.Users.Application.Commands.DeleteUser;
using DeveloperStore.Users.Application.Common.Repositories;
using DeveloperStore.Users.Domain.Entities;
using DeveloperStore.Users.Domain.Enums;

namespace DeveloperStore.Users.UnitTests.Application.Commands;

public class DeleteUserCommandHandlerTests
{
    private readonly IUserRepository _repo = Substitute.For<IUserRepository>();
    private DeleteUserCommandHandler CreateHandler() => new(_repo);

    [Fact]
    public async Task Handle_ShouldReturnDeleted_WhenUserExistsAndIsDeleted()
    {
        // Arrange
        var command = new DeleteUserCommand(1);
        var existingUser = User.Create(
            email: "user@email.com",
            username: "user",
            password: "pass",
            name: new("Ana", "Silva"),
            address: new("Rio", "Rua 1", 123, "00000-000", new("0", "0")),
            phone: "21999999999",
            status: UserStatus.Active,
            role: UserRole.Customer
        );

        _repo.GetByIdAsync(command.Id).Returns(existingUser);
        _repo.DeleteAsync(existingUser).Returns(true);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Deleted);
    }

    [Fact]
    public async Task Handle_ShouldReturnValidationError_WhenUserNotFound()
    {
        // Arrange
        var command = new DeleteUserCommand(99);
        _repo.GetByIdAsync(command.Id).Returns((User?)null);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
        result.FirstError.Code.Should().Be("User.Delete.NotFound");
        result.FirstError.Description.Should().Be("User with id 99 does not exist");
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenDeletionFails()
    {
        // Arrange
        var command = new DeleteUserCommand(2);
        var existingUser = User.Create(
            email: "fail@email.com",
            username: "failuser",
            password: "failpass",
            name: new("João", "Lima"),
            address: new("SP", "Rua 2", 456, "11111-111", new("1", "1")),
            phone: "21999988888",
            status: UserStatus.Inactive,
            role: UserRole.Manager
        );

        _repo.GetByIdAsync(command.Id).Returns(existingUser);
        _repo.DeleteAsync(existingUser).Returns(false);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
        result.FirstError.Description.Should().Be("Failed to delete user");
    }
}