using AutoMapper;
using FluentAssertions;
using NSubstitute;
using DeveloperStore.Products.Application.Common.Interfaces;
using DeveloperStore.Products.Application.Queries.GetProductById;
using DeveloperStore.Products.Contracts.Dtos;
using DeveloperStore.Products.Domain.Entities;
using DeveloperStore.Products.Domain.ValueObjects;
using ErrorOr;

namespace DeveloperStore.Products.UnitTests.Application.Queries;

public class GetProductByIdQueryHandlerTests
{
    private readonly IProductRepository _repo = Substitute.For<IProductRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private GetProductByIdQueryHandler CreateHandler() => new(_repo, _mapper);

    [Fact]
    public async Task Handle_ShouldReturnProductDto_WhenProductExists()
    {
        // Arrange
        var product = Product.Create(
            title: "Headset Gamer",
            price: 299.99m,
            description: "Headset com som 7.1",
            category: "Periféricos",
            image: "headset.png",
            rating: new Rating(4.6, 120)
        );

        var query = new GetProductByIdQuery(1);
        _repo.GetByIdAsync(query.Id).Returns(product);
        _mapper.Map<ProductDto>(product).Returns(new ProductDto
        {
            Title = "Headset Gamer",
            Price = 299.99m
        });

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Title.Should().Be("Headset Gamer");
        result.Value.Price.Should().Be(299.99m);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenProductNotFound()
    {
        // Arrange
        var query = new GetProductByIdQuery(42);
        _repo.GetByIdAsync(query.Id).Returns((Product?)null);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Description.Should().Be("Product not found.");
    }
}