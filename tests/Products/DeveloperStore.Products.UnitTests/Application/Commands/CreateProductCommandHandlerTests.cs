using AutoMapper;
using DeveloperStore.Products.Application.Commands.CreateProduct;
using DeveloperStore.Products.Application.Common.Interfaces;
using DeveloperStore.Products.Contracts.Dtos;
using DeveloperStore.Products.Domain.Entities;
using DeveloperStore.Products.Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace DeveloperStore.Products.UnitTests.Application.Commands;

public class CreateProductCommandHandlerTests
{
    private readonly IProductRepository _repo = Substitute.For<IProductRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private CreateProductCommandHandler CreateHandler() => new(_repo, _mapper);

    private CreateProductCommand BuildCommand() =>
        new CreateProductCommand
        {
            Title = "Notebook Dell",
            Price = 3500.99m,
            Description = "Notebook potente para desenvolvimento",
            Category = "Eletrônicos",
            Image = "image-url",
            Rating = new RatingDto { Rate = 0, Count = 0 }
        };

    [Fact]
    public async Task Handle_ShouldCreateProductAndReturnDto_WhenValid()
    {
        var command = BuildCommand();
        var expectedProduct = Product.Create(
            command.Title,
            command.Price,
            command.Description,
            command.Category,
            command.Image,
            new Rating(0, 0)
        );

        _mapper.Map<ProductDto>(expectedProduct).Returns(new ProductDto
        {
            Title = expectedProduct.Title,
            Price = expectedProduct.Price
        });

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Title.Should().Be("Notebook Dell");
        result.Value.Price.Should().Be(3500.99m);

        await _repo.Received(1).AddAsync(Arg.Any<Product>());
    }
}