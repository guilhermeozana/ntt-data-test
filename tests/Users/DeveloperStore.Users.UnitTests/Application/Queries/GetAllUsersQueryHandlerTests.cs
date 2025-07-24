using AutoMapper;
using FluentAssertions;
using NSubstitute;
using DeveloperStore.Shared.Contracts.Dtos;
using DeveloperStore.Shared.SharedKernel.Dtos;
using DeveloperStore.Shared.SharedKernel.Pagination;
using DeveloperStore.Users.Application.Common.Repositories;
using DeveloperStore.Users.Application.Queries.GetAllUsers;
using DeveloperStore.Users.Domain.Entities;
using DeveloperStore.Users.Domain.Enums;

namespace DeveloperStore.Users.UnitTests.Application.Queries;

public class GetAllUsersQueryHandlerTests
{
    private readonly IUserRepository _repo = Substitute.For<IUserRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private GetAllUsersQueryHandler CreateHandler() => new(_repo, _mapper);

    [Fact]
    public async Task Handle_ShouldReturnPagedUsers_WhenUsersExist()
    {
        // Arrange
        var criteria = new QueryCriteriaDto(
            page: 1,
            size: 2, // ⬅️ Mudado para simular 3 páginas (5 / 2 = 2.5 → 3)
            order: "username",
            filters: new Dictionary<string, string> { ["status"] = "active" },
            minValues: null,
            maxValues: null
        );

        var query = new GetAllUsersQuery(criteria);

        var domainUsers = new List<User>
        {
            User.Create("u1@email.com", "u1", "pwd", new("Ana", "Lima"), new("Rio", "R1", 1, "00000-000", new("0", "0")), "21999999999", UserStatus.Active, UserRole.Customer),
            User.Create("u2@email.com", "u2", "pwd", new("João", "Silva"), new("SP", "R2", 2, "11111-111", new("1", "1")), "21988888888", UserStatus.Inactive, UserRole.Manager)
        };

        var pagedResult = new PagedResult<User>
        {
            Items = domainUsers,
            TotalCount = 5
        };

        _repo.GetAllAsync(query.Criteria).Returns(pagedResult);

        _mapper.Map<List<UserDto>>(domainUsers).Returns(new List<UserDto>
        {
            new() { Email = "u1@email.com" },
            new() { Email = "u2@email.com" }
        });

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Data.Should().HaveCount(2);
        result.Value.TotalItems.Should().Be(5);
        result.Value.CurrentPage.Should().Be(1);
        result.Value.TotalPages.Should().Be(3);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyResult_WhenNoUsersExist()
    {
        //Arrange
        var criteria = new QueryCriteriaDto(
            page: 1,
            size: 5,
            order: "username",
            filters: new Dictionary<string, string> { ["status"] = "active" },
            minValues: null,
            maxValues: null
        );
        
        var query = new GetAllUsersQuery(criteria);

        _repo.GetAllAsync(query.Criteria).Returns(new PagedResult<User>
        {
            Items = new List<User>(),
            TotalCount = 0
        });

        _mapper.Map<List<UserDto>>(Arg.Any<List<User>>()).Returns(new List<UserDto>());

        var handler = CreateHandler();
        
        //Act
        var result = await handler.Handle(query, default);
        
        //Assert
        result.IsError.Should().BeFalse();
        result.Value.Data.Should().BeEmpty();
        result.Value.TotalItems.Should().Be(0);
        result.Value.TotalPages.Should().Be(0);
    }
}