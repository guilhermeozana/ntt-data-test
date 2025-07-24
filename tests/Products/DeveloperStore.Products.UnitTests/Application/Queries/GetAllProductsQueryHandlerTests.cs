using AutoMapper;
using FluentAssertions;
using NSubstitute;
using DeveloperStore.Products.Application.Common.Interfaces;
using DeveloperStore.Products.Application.Queries.GetAllProducts;
using DeveloperStore.Products.Contracts.Dtos;
using DeveloperStore.Products.Domain.Entities;
using DeveloperStore.Products.Domain.ValueObjects;
using DeveloperStore.Shared.SharedKernel.Dtos;
using DeveloperStore.Shared.SharedKernel.Pagination;

namespace DeveloperStore.Products.UnitTests.Application.Queries;

public class GetAllProductsQueryHandlerTests
{
    private readonly IProductRepository _repo = Substitute.For<IProductRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private GetAllProductsQueryHandler CreateHandler() => new(_repo, _mapper);

    [Fact]
    public async Task Handle_ShouldReturnPagedProducts_WhenProductsExist()
    {
        // Arrange
        var criteria = new QueryCriteriaDto(
            page: 2,
            size: 2,
            order: "title",
            filters: new Dictionary<string, string> { ["category"] = "Eletrônicos" },
            minValues: null,
            maxValues: null
        );

        var query = new GetAllProductsQuery(criteria);

        var domainProducts = new List<Product>
        {
            Product.Create("Monitor", 799.99m, "Full HD", "Eletrônicos", "monitor.png", new Rating(4.5, 120)),
            Product.Create("Teclado", 199.90m, "Mecânico RGB", "Eletrônicos", "teclado.png", new Rating(4.7, 85))
        };

        var pagedResult = new PagedResult<Product>
        {
            Items = domainProducts,
            TotalCount = 5 // Simula total em múltiplas páginas
        };

        _repo.GetAllAsync(criteria).Returns(pagedResult);

        _mapper.Map<List<ProductDto>>(domainProducts).Returns(new List<ProductDto>
        {
            new() { Title = "Monitor", Price = 799.99m },
            new() { Title = "Teclado", Price = 199.90m }
        });

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Data.Should().HaveCount(2);
        result.Value.TotalItems.Should().Be(5);
        result.Value.CurrentPage.Should().Be(2);
        result.Value.TotalPages.Should().Be(3); // 5 itens / 2 por página
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoProductsFound()
    {
        var criteria = new QueryCriteriaDto(1, 10, null, null, null, null);
        var query = new GetAllProductsQuery(criteria);

        _repo.GetAllAsync(criteria).Returns(new PagedResult<Product>
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