using AutoMapper;
using ErrorOr;
using FluentAssertions;
using NSubstitute;
using DeveloperStore.Products.Application.Commands.UpdateProduct;
using DeveloperStore.Products.Application.Common.Interfaces;
using DeveloperStore.Products.Contracts.Dtos;
using DeveloperStore.Products.Domain.Entities;
using DeveloperStore.Products.Domain.ValueObjects;

namespace DeveloperStore.Products.UnitTests.Application.Commands;

public class UpdateProductCommandHandlerTests
{
    private readonly IProductRepository _repo = Substitute.For<IProductRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private UpdateProductCommandHandler CreateHandler() => new(_repo, _mapper);

    private UpdateProductCommand BuildCommand() =>
        new UpdateProductCommand
        {
            Id = 1,
            Title = "Monitor Gamer",
            Price = 1799.99m,
            Description = "Monitor 144hz com painel IPS",
            Category = "Eletrônicos",
            Image = "img-monitor.png",
            Rating = new RatingDto { Rate = 4.8, Count = 215 }
        };

    [Fact]
    public async Task Handle_ShouldReturnError_WhenProductNotFound()
    {
        var command = BuildCommand();
        _repo.GetByIdAsync(command.Id).Returns((Product?)null);

        var handler = CreateHandler();
        var result = await handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Code.Should().Be("Product.Update.NotFound");
    }

    [Fact]
    public async Task Handle_ShouldUpdateProduct_WhenValid()
    {
        var command = BuildCommand();
        var product = Product.Create("Old", 100m, "Old desc", "Old Cat", "old.png", new Rating(0, 0));

        _repo.GetByIdAsync(command.Id).Returns(product);
        _mapper.Map<ProductDto>(product).Returns(new ProductDto
        {
            Title = command.Title,
            Price = command.Price
        });

        var handler = CreateHandler();
        var result = await handler.Handle(command, default);

        result.IsError.Should().BeFalse();
        result.Value.Title.Should().Be("Monitor Gamer");
        result.Value.Price.Should().Be(1799.99m);

        await _repo.Received(1).UpdateAsync(product);
    }
}