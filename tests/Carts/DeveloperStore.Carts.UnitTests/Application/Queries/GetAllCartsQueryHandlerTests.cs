using Xunit;
using FluentAssertions;
using NSubstitute;
using AutoMapper;
using DeveloperStore.Carts.Application.Queries.GetAllCarts;
using DeveloperStore.Carts.Application.Common.Interfaces;
using DeveloperStore.Carts.Contracts.Dtos;
using DeveloperStore.Carts.Domain.Entities;
using DeveloperStore.Shared.SharedKernel.Responses;
using DeveloperStore.Shared.SharedKernel.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using DeveloperStore.Shared.SharedKernel.Dtos;

namespace DeveloperStore.Carts.UnitTests.Application.Queries;

public class GetAllCartsQueryHandlerTests
{
    private readonly ICartRepository _repo = Substitute.For<ICartRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private GetAllCartsQueryHandler CreateHandler() => new(_repo, _mapper);

    private QueryCriteriaDto BuildCriteria(int page = 1, int size = 2) => new(page, size, null, null, null, null);

    [Fact]
    public async Task Handle_ShouldReturnPagedCarts_WhenCartsExist()
    {
        // Arrange
        var criteria = BuildCriteria();
        var query = new GetAllCartsQuery(criteria);

        var carts = new List<Cart>
        {
            Cart.Create(userId: 1, date: new DateTime(2025, 1, 1)),
            Cart.Create(userId: 2, date: new DateTime(2025, 1, 2))
        };

        var pagedResult = new PagedResult<Cart>
        {
            Items = carts,
            TotalCount = 5
        };

        _repo.GetPagedAsync(criteria).Returns(pagedResult);

        _mapper.Map<List<CartDto>>(carts).Returns(new List<CartDto>
        {
            new() { UserId = 1, Date = "2025-01-01", Products = new List<CartItemDto>() },
            new() { UserId = 2, Date = "2025-01-02", Products = new List<CartItemDto>() }
        });

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Data.Should().HaveCount(2);
        result.Value.TotalItems.Should().Be(5);
        result.Value.CurrentPage.Should().Be(1);
        result.Value.TotalPages.Should().Be(3); // 5 itens / 2 por página
    }

    [Fact]
    public async Task Handle_ShouldReturnEmpty_WhenNoCartsExist()
    {
        var criteria = BuildCriteria();
        var query = new GetAllCartsQuery(criteria);

        _repo.GetPagedAsync(criteria).Returns(new PagedResult<Cart>
        {
            Items = new List<Cart>(),
            TotalCount = 0
        });

        _mapper.Map<List<CartDto>>(Arg.Any<List<Cart>>()).Returns(new List<CartDto>());

        var handler = CreateHandler();
        var result = await handler.Handle(query, default);

        result.IsError.Should().BeFalse();
        result.Value.Data.Should().BeEmpty();
        result.Value.TotalItems.Should().Be(0);
        result.Value.TotalPages.Should().Be(0);
    }
}