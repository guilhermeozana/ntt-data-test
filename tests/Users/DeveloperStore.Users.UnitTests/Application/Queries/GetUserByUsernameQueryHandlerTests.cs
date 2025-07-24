using AutoMapper;
using DeveloperStore.Shared.Contracts.Dtos;
using DeveloperStore.Users.Application.Common.Repositories;
using DeveloperStore.Users.Application.Queries.GetUserByUsername;
using DeveloperStore.Users.Domain.Entities;
using DeveloperStore.Users.Domain.Enums;
using DeveloperStore.Users.Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace DeveloperStore.Users.UnitTests.Application.Queries;

public class GetUserByUsernameQueryHandlerTests
{
    private readonly IUserRepository _repo = Substitute.For<IUserRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private GetUserByUsernameQueryHandler CreateHandler() => new(_repo, _mapper);

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserByUsernameIsNotFound()
    {
        var query = new GetUserByUsernameQuery(username: "admin");

        _repo.GetByUsernameAsync(query.Username).Returns((User?)null);

        var handler = CreateHandler();
        var result = await handler.Handle(query, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("User.NotFound");
        result.FirstError.Description.Should().Be("User not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnUserDto_WhenUserByUsernameIsFound()
    {
        var user = User.Create(
            email: "ana@email.com",
            username: "ana123",
            password: "senhaSegura",
            name: new Name("Ana", "Beatriz"),
            address: new Address("RJ", "Rua 1", 10, "22222-222", new Geolocation("0", "0")),
            phone: "21999999999",
            status: UserStatus.Active,
            role: UserRole.Customer
        );

        var query = new GetUserByUsernameQuery("ana123");
        _repo.GetByUsernameAsync(query.Username).Returns(user);
        _mapper.Map<UserDto>(user).Returns(new UserDto { Email = user.Email });

        var handler = CreateHandler();
        var result = await handler.Handle(query, default);

        result.IsError.Should().BeFalse();
        result.Value.Email.Should().Be("ana@email.com");
    }
}