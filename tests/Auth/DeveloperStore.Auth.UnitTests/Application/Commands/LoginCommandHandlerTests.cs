using FluentAssertions;
using NSubstitute;
using DeveloperStore.Auth.Application.Commands.Login;
using DeveloperStore.Auth.Application.External;
using DeveloperStore.Auth.Application.Security;
using DeveloperStore.Auth.Contracts.Security;
using DeveloperStore.Shared.Contracts.Dtos;

namespace DeveloperStore.Auth.UnitTests.Application.Commands;

public class LoginCommandHandlerTests
{
    private readonly IUsersApiClient _usersApiClient = Substitute.For<IUsersApiClient>();
    private readonly IJwtProvider _jwtProvider = Substitute.For<IJwtProvider>();
    private LoginCommandHandler CreateHandler() => new(_usersApiClient, _jwtProvider);

    [Fact]
    public async Task Handle_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var request = new LoginCommand("john", "secure123");
        var user = new UserDto
        {
            Id = 1,
            Username = "john",
            Password = "secure123",
            Email = "john@example.com",
            Role = "admin"
        };

        _usersApiClient.GetByUsernameAsync("john").Returns(user);
        _jwtProvider.Generate(Arg.Any<TokenPayload>()).Returns("mocked-jwt-token");

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(request, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be("mocked-jwt-token");
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserNotFound()
    {
        var request = new LoginCommand("invalid", "123");

        _usersApiClient.GetByUsernameAsync("invalid").Returns((UserDto?)null);

        var handler = CreateHandler();
        var result = await handler.Handle(request, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Auth.InvalidCredentials");
        result.FirstError.Description.Should().Be("Invalid username or password.");
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenPasswordIsIncorrect()
    {
        var request = new LoginCommand("john", "wrong-pass");
        var user = new UserDto
        {
            Id = 2,
            Username = "john",
            Password = "correct-pass",
            Email = "john@example.com",
            Role = "user"
        };

        _usersApiClient.GetByUsernameAsync("john").Returns(user);

        var handler = CreateHandler();
        var result = await handler.Handle(request, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Auth.InvalidCredentials");
        result.FirstError.Description.Should().Be("Invalid username or password.");
    }
}