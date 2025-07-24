using ErrorOr;
using FluentAssertions;
using NSubstitute;
using DeveloperStore.Carts.Application.Commands.DeleteCart;
using DeveloperStore.Carts.Application.Common.Interfaces;
using DeveloperStore.Carts.Domain.Entities;

namespace DeveloperStore.Carts.UnitTests.Application.Commands;

public class DeleteCartCommandHandlerTests
{
    private readonly ICartRepository _repo = Substitute.For<ICartRepository>();
    private DeleteCartCommandHandler CreateHandler() => new(_repo);

    [Fact]
    public async Task Handle_ShouldReturnDeleted_WhenCartExists()
    {
        // Arrange
        var cart = Cart.Create(userId: 10, date: DateTime.UtcNow);
        var command = new DeleteCartCommand(cart.Id);

        _repo.GetByIdAsync(command.Id).Returns(cart);
        _repo.DeleteAsync(cart).Returns(Task.FromResult(true));

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Deleted);
        await _repo.Received(1).DeleteAsync(cart);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenCartDoesNotExist()
    {
        // Arrange
        var command = new DeleteCartCommand(id: 999);
        _repo.GetByIdAsync(command.Id).Returns((Cart?)null);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Code.Should().Be("Cart.NotFound");
        result.FirstError.Description.Should().Be("Cart with id 999 was not found.");
    }
}