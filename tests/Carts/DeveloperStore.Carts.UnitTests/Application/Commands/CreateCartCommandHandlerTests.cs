using AutoMapper;
using FluentAssertions;
using NSubstitute;
using DeveloperStore.Carts.Application.Commands.CreateCart;
using DeveloperStore.Carts.Application.Common.Interfaces;
using DeveloperStore.Carts.Contracts.Dtos;
using DeveloperStore.Carts.Domain.Entities;

namespace DeveloperStore.Carts.UnitTests.Application.Commands;

public class CreateCartCommandHandlerTests
{
    private readonly ICartRepository _repo = Substitute.For<ICartRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private CreateCartCommandHandler CreateHandler() => new(_repo, _mapper);

    private CreateCartCommand BuildCommand()
    {
        return new CreateCartCommand
        {
            UserId = 99,
            Date = "2025-07-23",
            Products = new List<CartItemDto>
            {
                new() { ProductId = 1001, Quantity = 2 },
                new() { ProductId = 1002, Quantity = 1 }
            }
        };
    }

    [Fact]
    public async Task Handle_ShouldCreateCartAndReturnDto_WhenCommandIsValid()
    {
        var command = BuildCommand();

        var expectedCart = Cart.Create(command.UserId, DateTime.Parse(command.Date));
        expectedCart.AddItem(1001, 2);
        expectedCart.AddItem(1002, 1);

        _mapper.Map<CartDto>(expectedCart).Returns(new CartDto
        {
            UserId = 99,
            Date = command.Date,
            Products = command.Products
        });

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.UserId.Should().Be(99);
        result.Value.Products.Should().HaveCount(2);
        await _repo.Received(1).AddAsync(Arg.Any<Cart>());
    }

    [Fact]
    public async Task Handle_ShouldAddAllItemsToCart()
    {
        var command = BuildCommand();
        var handler = CreateHandler();

        var result = await handler.Handle(command, default);

        var cartArg = Arg.Is<Cart>(c =>
            Enumerable.Any(c.Products, i => i.ProductId == 1001 && i.Quantity == 2) &&
            Enumerable.Any(c.Products, i => i.ProductId == 1002 && i.Quantity == 1)
        );

        await _repo.Received(1).AddAsync(cartArg);
    }
}