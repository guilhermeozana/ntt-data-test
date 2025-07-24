using System.Text.Json;
using AutoMapper;
using DeveloperStore.Auth.Api.Controllers;
using DeveloperStore.Auth.Application.Commands.Login;
using DeveloperStore.Auth.Contracts.Requests;
using DeveloperStore.Auth.Contracts.Response;
using ErrorOr;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace DeveloperStore.Auth.UnitTests.Api.Controllers;

public class AuthControllerTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _controller = new AuthController(_mediator, _mapper);
    }

    private static JsonElement ToJsonElement(object value)
    {
        var json = JsonSerializer.Serialize(value);
        return JsonDocument.Parse(json).RootElement;
    }

    [Fact]
    public async Task Login_ShouldReturnOK_WhenLoginIsSuccessful()
    {
        // Arrange
        var request = new LoginRequest { Username = "test", Password = "123456" };
        var command = new LoginCommand(request.Username, request.Password);
        var token = "jwt-token-123";

        _mapper.Map<LoginCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>())
            .Returns("jwt-token-123");

        // Act
        var result = await _controller.Login(request);

        // Assert
        var okObjectResult = result as OkObjectResult;
        okObjectResult.Should().NotBeNull();
        okObjectResult!.StatusCode.Should().Be(200);

        var response = okObjectResult.Value as LoginResponse;
        response.Should().NotBeNull();
        response!.Token.Should().Be(token);
    }

    [Fact]
    public async Task Login_ShouldReturnProblem_WhenLoginFails()
    {
        // Arrange
        var request = new LoginRequest { Username = "test", Password = "wrong" };
        var command = new LoginCommand(request.Username, request.Password);
        var error = Error.Unauthorized( "Invalid credentials", "Invalid credentials");

        _mapper.Map<LoginCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>())
            .Returns(new List<Error>{error});

        // Act
        var result = await _controller.Login(request);

        // Assert
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult!.StatusCode.Should().Be(401);

        var root = ToJsonElement(objectResult.Value!);
        root.GetProperty("type").GetString().Should().Be("AuthenticationError");
        root.GetProperty("error").GetString().Should().Be("Invalid authentication token");
        root.GetProperty("detail").GetString().Should().Be("Invalid credentials");
    }
}