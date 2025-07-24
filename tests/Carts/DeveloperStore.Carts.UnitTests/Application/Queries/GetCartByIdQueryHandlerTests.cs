using AutoMapper;
using FluentAssertions;
using NSubstitute;
using DeveloperStore.Carts.Application.Common.Interfaces;
using DeveloperStore.Carts.Application.Queries.GetCartById;
using DeveloperStore.Carts.Contracts.Dtos;
using DeveloperStore.Carts.Domain.Entities;
using ErrorOr;

namespace DeveloperStore.Carts.UnitTests.Application.Queries;

public class GetCartByIdQueryHandlerTests
{
    private readonly ICartRepository _repo = Substitute.For<ICartRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private GetCartByIdQueryHandler CreateHandler() => new(_repo, _mapper);

    [Fact]
    public async Task Handle_ShouldReturnMappedDto_WhenCartExists()
    {
        var cart = Cart.Create(userId: 1, date: new DateTime(2025, 1, 1));
        cart.AddItem(1001, 2);

        var query = new GetCartByIdQuery(cart.Id);

        _repo.GetByIdAsync(query.Id).Returns(cart);

        _mapper.Map<CartDto>(cart).Returns(new CartDto
        {
            UserId = 1,
            Date = "2025-01-01",
            Products = new List<CartItemDto>
            {
                new() { ProductId = 1001, Quantity = 2 }
            }
        });

        var handler = CreateHandler();
        var result = await handler.Handle(query, default);

        result.IsError.Should().BeFalse();
        result.Value.UserId.Should().Be(1);
        result.Value.Date.Should().Be("2025-01-01");
        result.Value.Products.Should().ContainSingle(i => i.ProductId == 1001 && i.Quantity == 2);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenCartNotFound()
    {
        // Arrange
        var query = new GetCartByIdQuery(999);
        _repo.GetByIdAsync(query.Id).Returns((Cart?)null);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Description.Should().Be("Cart not found.");
    }
}