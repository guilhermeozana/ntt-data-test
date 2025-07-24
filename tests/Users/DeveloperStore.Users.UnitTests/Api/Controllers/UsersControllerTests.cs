using AutoMapper;
using DeveloperStore.Shared.Api.Queries;
using DeveloperStore.Shared.Contracts.Dtos;
using DeveloperStore.Shared.SharedKernel.Responses;
using DeveloperStore.Users.Api.Controllers;
using DeveloperStore.Users.Application.Commands.CreateUser;
using DeveloperStore.Users.Application.Commands.DeleteUser;
using DeveloperStore.Users.Application.Commands.UpdateUser;
using DeveloperStore.Users.Application.Queries.GetAllUsers;
using DeveloperStore.Users.Application.Queries.GetUserById;
using DeveloperStore.Users.Application.Queries.GetUserByUsername;
using DeveloperStore.Users.Contracts.Requests;
using ErrorOr;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace DeveloperStore.Users.UnitTests.Api.Controllers;

public class UsersControllerTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _controller = new UsersController(_mediator, _mapper);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WhenQueryIsValid()
    {
        var request = new RequestQueryOptions { Page = 1, Size = 10, Order = "username" };

        var expected = new PagedResponse<UserDto>(
            data: new List<UserDto> { new UserDto { Id = 1, Username = "user1", Email = "user1@example.com" } },
            totalItems: 1,
            currentPage: 1,
            totalPages: 1
        );

        _mediator.Send(Arg.Any<GetAllUsersQuery>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        var result = await _controller.GetAll(request);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenUserExists()
    {
        var user = new UserDto { Id = 42, Username = "user42", Email = "u42@example.com" };

        _mediator.Send(Arg.Any<GetUserByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(user);

        var result = await _controller.GetById(42);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task GetByUsername_ShouldReturnOk_WhenUserExists()
    {
        var user = new UserDto { Id = 99, Username = "johndoe", Email = "john@example.com" };

        _mediator.Send(Arg.Any<GetUserByUsernameQuery>(), Arg.Any<CancellationToken>())
            .Returns(user);

        var result = await _controller.GetByUsername("johndoe");

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAt_WhenSuccessful()
    {
        var request = new CreateUserRequest
        {
            Email = "new@example.com",
            Username = "newuser",
            Password = "secure",
            Name = new NameDto { Firstname = "New", Lastname = "User" },
            Address = new AddressDto { City = "Rio", Street = "Main", Number = 100 },
            Phone = "123456789",
            Status = "Active",
            Role = "Customer"
        };

        var command = new CreateUserCommand
        {
            Email = request.Email,
            Username = request.Username,
            Password = request.Password,
            Name = request.Name,
            Address = request.Address,
            Phone = request.Phone,
            Status = request.Status,
            Role = request.Role
        };

        var createdUser = new UserDto
        {
            Id = 10,
            Username = request.Username,
            Email = request.Email
        };

        _mapper.Map<CreateUserCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>())
            .Returns(createdUser);

        var result = await _controller.Create(request);

        var created = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        created.Value.Should().BeEquivalentTo(createdUser);
        created.RouteValues!["id"].Should().Be(createdUser.Id);
    }

    [Fact]
    public async Task Update_ShouldReturnOk_WhenSuccessful()
    {
        var request = new UpdateUserRequest
        {
            Id = 5,
            Email = "update@example.com",
            Username = "updateduser",
            Password = "newpass",
            Name = new NameDto { Firstname = "Updated", Lastname = "User" },
            Address = new AddressDto { City = "SP", Street = "Rua Nova", Number = 50 },
            Phone = "987654321",
            Status = "Active",
            Role = "Admin"
        };

        var command = new UpdateUserCommand
        {
            Id = 5,
            Email = request.Email,
            Username = request.Username,
            Password = request.Password,
            Name = request.Name,
            Address = request.Address,
            Phone = request.Phone,
            Status = request.Status,
            Role = request.Role
        };

        var updatedUser = new UserDto
        {
            Id = 5,
            Username = request.Username,
            Email = request.Email
        };

        _mapper.Map<UpdateUserCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>())
            .Returns(updatedUser);

        var result = await _controller.Update(5, request);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(updatedUser);
    }

    [Fact]
    public async Task Delete_ShouldReturnOk_WhenSuccessful()
    {
        _mediator.Send(Arg.Any<DeleteUserCommand>(), Arg.Any<CancellationToken>())
            .Returns(new Deleted());

        var result = await _controller.Delete(3);

        result.Should().BeOfType<OkObjectResult>();
    }
}