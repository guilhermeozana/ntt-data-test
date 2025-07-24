using ErrorOr;
using FluentAssertions;
using NSubstitute;
using DeveloperStore.Products.Application.Commands.DeleteProduct;
using DeveloperStore.Products.Application.Common.Interfaces;
using DeveloperStore.Products.Domain.Entities;
using DeveloperStore.Products.Domain.ValueObjects;

namespace DeveloperStore.Products.UnitTests.Application.Commands;

public class DeleteProductCommandHandlerTests
{
    private readonly IProductRepository _repo = Substitute.For<IProductRepository>();
    private DeleteProductCommandHandler CreateHandler() => new(_repo);

    [Fact]
    public async Task Handle_ShouldReturnDeleted_WhenProductExistsAndIsDeleted()
    {
        var command = new DeleteProductCommand(1);
        var product = Product.Create(
            title: "Mouse",
            price: 99.99m,
            description: "Mouse gamer",
            category: "Periféricos",
            image: "image-url",
            rating: new Rating(4.9, 300)
        );

        _repo.GetByIdAsync(command.Id).Returns(product);
        _repo.DeleteAsync(product).Returns(true);

        var handler = CreateHandler();
        var result = await handler.Handle(command, default);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Deleted);
    }

    [Fact]
    public async Task Handle_ShouldReturnValidationError_WhenProductNotFound()
    {
        var command = new DeleteProductCommand(99);
        _repo.GetByIdAsync(command.Id).Returns((Product?)null);

        var handler = CreateHandler();
        var result = await handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Product.Delete.NotFound");
        result.FirstError.Description.Should().Be("Product with id 99 does not exist");
    }

    [Fact]
    public async Task Handle_ShouldReturnUnexpectedError_WhenDeleteFails()
    {
        var command = new DeleteProductCommand(2);
        var product = Product.Create(
            title: "Teclado",
            price: 199.90m,
            description: "Teclado mecânico",
            category: "Periféricos",
            image: "image-url",
            rating: new Rating(4.6, 150)
        );

        _repo.GetByIdAsync(command.Id).Returns(product);
        _repo.DeleteAsync(product).Returns(false);

        var handler = CreateHandler();
        var result = await handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Unexpected);
        result.FirstError.Code.Should().Be("Product.Delete.FailedToDelete");
        result.FirstError.Description.Should().Be("Failed to delete product");
    }
}