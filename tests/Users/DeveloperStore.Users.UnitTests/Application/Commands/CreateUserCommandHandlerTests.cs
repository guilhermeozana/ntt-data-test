using AutoMapper;
using FluentAssertions;
using NSubstitute;
using DeveloperStore.Shared.Contracts.Dtos;
using DeveloperStore.Users.Application.Commands.CreateUser;
using DeveloperStore.Users.Application.Common.Repositories;
using DeveloperStore.Users.Domain.Entities;
using DeveloperStore.Users.Domain.Enums;

namespace DeveloperStore.Users.UnitTests.Application.Commands;

public class CreateUserCommandHandlerTests
{
    private readonly IUserRepository _repo = Substitute.For<IUserRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private CreateUserCommandHandler CreateHandler() => new(_repo, _mapper);

    private CreateUserCommand BuildCommand() =>
        new CreateUserCommand
        {
            Email = "guilherme@email.com",
            Username = "gui_lima",
            Password = "secure@2025",
            Phone = "21-99999-9999",
            Status = "Active",
            Role = "Admin",
            Name = new NameDto
            {
                Firstname = "Guilherme",
                Lastname = "Lima"
            },
            Address = new AddressDto
            {
                City = "Rio de Janeiro",
                Street = "Rua das Flores",
                Number = 123,
                Zipcode = "20000-000",
                Geolocation = new GeolocationDto
                {
                    Lat = "22.9068",
                    Long = "43.1729"
                }
            }
        };

    [Fact]
    public async Task Handle_ShouldCreateUserAndReturnDto_WhenValid()
    {
        // Arrange
        var command = BuildCommand();
        var handler = CreateHandler();

        _mapper.Map<UserDto>(Arg.Any<User>())
               .Returns(new UserDto { Email = command.Email });

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Email.Should().Be("guilherme@email.com");

        await _repo.Received(1).AddAsync(Arg.Any<User>());
    }

    [Theory]
    [InlineData("Active", UserStatus.Active)]
    [InlineData("inactive", UserStatus.Inactive)]
    [InlineData("SUSPENDED", UserStatus.Suspended)]
    public async Task Handle_ShouldParseUserStatus_FromString(string input, UserStatus expected)
    {
        var command = BuildCommand();
        command.Status = input;

        var handler = CreateHandler();
        await handler.Handle(command, default);

        await _repo.Received(1).AddAsync(
            Arg.Is<User>(u => u.Status == expected)
        );
    }

    [Theory]
    [InlineData("Admin", UserRole.Admin)]
    [InlineData("manager", UserRole.Manager)]
    [InlineData("CUSTOMER", UserRole.Customer)]
    public async Task Handle_ShouldParseUserRole_FromString(string input, UserRole expected)
    {
        var command = BuildCommand();
        command.Role = input;

        var handler = CreateHandler();
        await handler.Handle(command, default);

        await _repo.Received(1).AddAsync(
            Arg.Is<User>(u => u.Role == expected)
        );
    }
}