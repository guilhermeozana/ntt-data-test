using AutoMapper;
using FluentAssertions;
using NSubstitute;
using DeveloperStore.Products.Application.Common.Interfaces;
using DeveloperStore.Products.Application.Queries.GetProductsByCategory;
using DeveloperStore.Products.Contracts.Dtos;
using DeveloperStore.Products.Domain.Entities;
using DeveloperStore.Products.Domain.ValueObjects;
using DeveloperStore.Shared.SharedKernel.Dtos;
using DeveloperStore.Shared.SharedKernel.Pagination;

namespace DeveloperStore.Products.UnitTests.Application.Queries;

public class GetProductsByCategoryQueryHandlerTests
{
    private readonly IProductRepository _repo = Substitute.For<IProductRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private GetProductsByCategoryQueryHandler CreateHandler() => new(_repo, _mapper);

    [Fact]
    public async Task Handle_ShouldReturnFilteredPagedProducts_WhenProductsExistInCategory()
    {
        // Arrange
        var criteria = new QueryCriteriaDto(
            page: 1,
            size: 2,
            order: "title",
            filters: null,
            minValues: null,
            maxValues: null
        );

        var query = new GetProductsByCategoryQuery("Eletrônicos", criteria);

        var domainProducts = new List<Product>
        {
            Product.Create("Monitor", 899.99m, "Full HD", "Eletrônicos", "img1.png", new Rating(4.5, 110)),
            Product.Create("Mouse", 199.99m, "Wireless", "Eletrônicos", "img2.png", new Rating(4.7, 75))
        };

        var pagedResult = new PagedResult<Product>
        {
            Items = domainProducts,
            TotalCount = 5
        };

        _repo.GetByCategoryAsync("Eletrônicos", criteria).Returns(pagedResult);

        _mapper.Map<List<ProductDto>>(domainProducts).Returns(new List<ProductDto>
        {
            new() { Title = "Monitor", Price = 899.99m },
            new() { Title = "Mouse", Price = 199.99m }
        });

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Data.Should().HaveCount(2);
        result.Value.TotalItems.Should().Be(5);
        result.Value.CurrentPage.Should().Be(1);
        result.Value.TotalPages.Should().Be(3); // 5 items / 2 per page
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyResult_WhenCategoryHasNoProducts()
    {
        var criteria = new QueryCriteriaDto(1, 5, null, null, null, null);
        var query = new GetProductsByCategoryQuery("Esportes", criteria);

        _repo.GetByCategoryAsync("Esportes", criteria).Returns(new PagedResult<Product>
        {
            Items = new List<Product>(),
            TotalCount = 0
        });

        _mapper.Map<List<ProductDto>>(Arg.Any<List<Product>>()).Returns(new List<ProductDto>());

        var handler = CreateHandler();
        var result = await handler.Handle(query, default);

        result.IsError.Should().BeFalse();
        result.Value.Data.Should().BeEmpty();
        result.Value.TotalItems.Should().Be(0);
        result.Value.TotalPages.Should().Be(0);
    }
}