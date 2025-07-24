using AutoMapper;
using FluentAssertions;
using NSubstitute;
using DeveloperStore.Shared.Contracts.Dtos;
using DeveloperStore.Users.Application.Commands.UpdateUser;
using DeveloperStore.Users.Application.Common.Repositories;
using DeveloperStore.Users.Domain.Entities;
using DeveloperStore.Users.Domain.Enums;
using DeveloperStore.Users.Domain.ValueObjects;

namespace DeveloperStore.Users.UnitTests.Application.Commands;

public class UpdateUserCommandHandlerTests
{
    private readonly IUserRepository _repo = Substitute.For<IUserRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private UpdateUserCommandHandler CreateHandler() => new(_repo, _mapper);

    private UpdateUserCommand BuildCommand() =>
        new UpdateUserCommand
        {
            Id = 1,
            Email = "updated@email.com",
            Username = "updated_user",
            Password = "new_password",
            Phone = "21988889999",
            Status = "Suspended",
            Role = "Manager",
            Name = new NameDto
            {
                Firstname = "Lucas",
                Lastname = "Silva"
            },
            Address = new AddressDto
            {
                City = "São Paulo",
                Street = "Av. Paulista",
                Number = 500,
                Zipcode = "01311-000",
                Geolocation = new GeolocationDto
                {
                    Lat = "-23.561684",
                    Long = "-46.655981"
                }
            }
        };

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserNotFound()
    {
        var command = BuildCommand();
        _repo.GetByIdAsync(command.Id).Returns((User?)null);

        var handler = CreateHandler();
        var result = await handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("User.NotFound");
    }

    [Fact]
    public async Task Handle_ShouldUpdateUser_WhenValid()
    {
        var command = BuildCommand();
        var existingUser = User.Create(
            email: "old@email.com",
            username: "olduser",
            password: "oldpass",
            name: new Name("Ana", "Souza"),
            address: new Address("RJ", "Rua 1", 123, "20000-000", new Geolocation("0", "0")),
            phone: "21999999999",
            status: UserStatus.Active,
            role: UserRole.Customer
        );

        _repo.GetByIdAsync(command.Id).Returns(existingUser);
        _mapper.Map<UserDto>(existingUser).Returns(new UserDto { Email = command.Email });

        var handler = CreateHandler();
        var result = await handler.Handle(command, default);

        result.IsError.Should().BeFalse();
        result.Value.Email.Should().Be("updated@email.com");

        await _repo.Received(1).UpdateAsync(existingUser);
    }

    [Theory]
    [InlineData("Active", UserStatus.Active)]
    [InlineData("INACTIVE", UserStatus.Inactive)]
    [InlineData("suspended", UserStatus.Suspended)]
    public async Task Handle_ShouldParseUserStatusCorrectly(string input, UserStatus expected)
    {
        var command = BuildCommand();
        command.Status = input;

        var existingUser = User.Create(
            email: "temp@email.com",
            username: "temp",
            password: "temp",
            name: new Name("Temp", "User"),
            address: new Address("Cidade", "Rua", 1, "00000-000", new Geolocation("0", "0")),
            phone: "999999999",
            status: UserStatus.Active,
            role: UserRole.Customer
        );

        _repo.GetByIdAsync(command.Id).Returns(existingUser);
        _mapper.Map<UserDto>(Arg.Any<User>()).Returns(new UserDto());

        var handler = CreateHandler();
        await handler.Handle(command, default);

        existingUser.Status.Should().Be(expected);
    }

    [Theory]
    [InlineData("Customer", UserRole.Customer)]
    [InlineData("ADMIN", UserRole.Admin)]
    [InlineData("manager", UserRole.Manager)]
    public async Task Handle_ShouldParseUserRoleCorrectly(string input, UserRole expected)
    {
        var command = BuildCommand();
        command.Role = input;

        var existingUser = User.Create(
            email: "temp@email.com",
            username: "temp",
            password: "temp",
            name: new Name("Temp", "User"),
            address: new Address("Cidade", "Rua", 1, "00000-000", new Geolocation("0", "0")),
            phone: "999999999",
            status: UserStatus.Active,
            role: UserRole.Customer
        );

        _repo.GetByIdAsync(command.Id).Returns(existingUser);
        _mapper.Map<UserDto>(Arg.Any<User>()).Returns(new UserDto());

        var handler = CreateHandler();
        await handler.Handle(command, default);

        existingUser.Role.Should().Be(expected);
    }
}