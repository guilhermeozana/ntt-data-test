using FluentAssertions;
using NSubstitute;
using DeveloperStore.Products.Application.Common.Interfaces;
using DeveloperStore.Products.Application.Queries.GetAllCategories;

namespace DeveloperStore.Products.UnitTests.Application.Queries;

public class GetAllCategoriesQueryHandlerTests
{
    private readonly IProductRepository _repo = Substitute.For<IProductRepository>();
    private readonly GetAllCategoriesQueryHandler _handler;

    public GetAllCategoriesQueryHandlerTests()
    {
        _handler = new GetAllCategoriesQueryHandler(_repo);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfCategories_WhenCategoriesExist()
    {
        // Arrange
        var expectedCategories = new List<string> { "Eletrônicos", "Livros", "Moda" };
        _repo.GetAllCategoriesAsync().Returns(expectedCategories);

        var query = new GetAllCategoriesQuery();

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(expectedCategories);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoCategoriesFound()
    {
        // Arrange
        _repo.GetAllCategoriesAsync().Returns(new List<string>());
        var query = new GetAllCategoriesQuery();

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEmpty();
    }
}