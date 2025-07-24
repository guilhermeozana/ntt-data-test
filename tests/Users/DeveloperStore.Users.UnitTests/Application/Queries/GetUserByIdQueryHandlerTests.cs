using AutoMapper;
using FluentAssertions;
using NSubstitute;
using DeveloperStore.Shared.Contracts.Dtos;
using DeveloperStore.Users.Application.Common.Repositories;
using DeveloperStore.Users.Application.Queries.GetUserById;
using DeveloperStore.Users.Domain.Entities;
using DeveloperStore.Users.Domain.Enums;
using DeveloperStore.Users.Domain.ValueObjects;

namespace DeveloperStore.Users.UnitTests.Application.Queries;

public class GetUserByIdQueryHandlerTests
{
    private readonly IUserRepository _repo = Substitute.For<IUserRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private GetUserByIdQueryHandler CreateHandler() => new(_repo, _mapper);

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserByIdIsNotFound()
    {
        var query = new GetUserByIdQuery(id: 42);

        _repo.GetByIdAsync(query.Id).Returns((User?)null);

        var handler = CreateHandler();
        var result = await handler.Handle(query, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("User.NotFound");
        result.FirstError.Description.Should().Be("User not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnUserDto_WhenUserByIdIsFound()
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

        var query = new GetUserByIdQuery(1);
        _repo.GetByIdAsync(query.Id).Returns(user);
        _mapper.Map<UserDto>(user).Returns(new UserDto { Email = user.Email });

        var handler = CreateHandler();
        var result = await handler.Handle(query, default);

        result.IsError.Should().BeFalse();
        result.Value.Email.Should().Be("ana@email.com");
    }
}