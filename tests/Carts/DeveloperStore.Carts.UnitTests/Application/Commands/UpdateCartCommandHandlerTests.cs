using AutoMapper;
using ErrorOr;
using FluentAssertions;
using NSubstitute;
using DeveloperStore.Carts.Application.Commands.UpdateCart;
using DeveloperStore.Carts.Application.Common.Interfaces;
using DeveloperStore.Carts.Contracts.Dtos;
using DeveloperStore.Carts.Domain.Entities;

namespace DeveloperStore.Carts.UnitTests.Application.Commands;

public class UpdateCartCommandHandlerTests
{
    private readonly ICartRepository _repo = Substitute.For<ICartRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private UpdateCartCommandHandler CreateHandler() => new(_repo, _mapper);

    private UpdateCartCommand BuildValidCommand() =>
        new UpdateCartCommand
        {
            Id = 1,
            UserId = 99,
            Date = "2025-07-23",
            Products = new List<CartItemDto>
            {
                new() { ProductId = 1001, Quantity = 2 },
                new() { ProductId = 1002, Quantity = 1 }
            }
        };

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenCartDoesNotExist()
    {
        var command = BuildValidCommand();
        _repo.GetByIdAsync(command.Id).Returns((Cart?)null);

        var handler = CreateHandler();
        var result = await handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Cart.NotFound");
        result.FirstError.Description.Should().Be("Cart not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnValidationError_WhenQuantityExceedsLimit()
    {
        var command = BuildValidCommand();
        command.Products[0].Quantity = 99; // inválido (>20)

        var cart = Cart.Create(command.UserId, DateTime.Parse(command.Date));
        _repo.GetByIdAsync(command.Id).Returns(cart);

        var handler = CreateHandler();
        var result = await handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
        result.FirstError.Description.Should().Be("Cannot purchase more than 20 identical items.");
    }

    [Fact]
    public async Task Handle_ShouldUpdateCartAndReturnDto_WhenCommandIsValid()
    {
        var command = BuildValidCommand();
        var cart = Cart.Create(1, DateTime.UtcNow);

        _repo.GetByIdAsync(command.Id).Returns(cart);

        _mapper.Map<CartDto>(cart).Returns(new CartDto
        {
            UserId = command.UserId,
            Date = command.Date,
            Products = command.Products
        });

        var handler = CreateHandler();
        var result = await handler.Handle(command, default);

        result.IsError.Should().BeFalse();
        result.Value.UserId.Should().Be(99);
        result.Value.Products.Should().HaveCount(2);
        await _repo.Received(1).UpdateAsync(cart);
    }
}