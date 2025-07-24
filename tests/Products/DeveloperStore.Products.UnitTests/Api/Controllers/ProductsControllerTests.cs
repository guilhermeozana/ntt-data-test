using AutoMapper;
using DeveloperStore.Products.Api.Controllers;
using DeveloperStore.Products.Application.Commands.CreateProduct;
using DeveloperStore.Products.Application.Commands.DeleteProduct;
using DeveloperStore.Products.Application.Commands.UpdateProduct;
using DeveloperStore.Products.Application.Queries.GetAllCategories;
using DeveloperStore.Products.Application.Queries.GetAllProducts;
using DeveloperStore.Products.Application.Queries.GetProductById;
using DeveloperStore.Products.Application.Queries.GetProductsByCategory;
using DeveloperStore.Products.Contracts.Dtos;
using DeveloperStore.Products.Contracts.Requests;
using DeveloperStore.Shared.Api.Queries;
using DeveloperStore.Shared.SharedKernel.Responses;
using ErrorOr;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace DeveloperStore.Products.UnitTests.Api.Controllers;

public class ProductsControllerTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _controller = new ProductsController(_mediator, _mapper);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WhenQueryIsValid()
    {
        var queryOptions = new RequestQueryOptions { Page = 1, Size = 10, Order = "title" };

        var response = new PagedResponse<ProductDto>(
            data: new List<ProductDto> { new ProductDto { Id = 1, Title = "Laptop", Price = 3000, Category = "tech" } },
            totalItems: 1,
            currentPage: 1,
            totalPages: 1
        );

        _mediator.Send(Arg.Any<GetAllProductsQuery>(), Arg.Any<CancellationToken>())
            .Returns(response);

        var result = await _controller.GetAll(queryOptions);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenProductExists()
    {
        var product = new ProductDto { Id = 3, Title = "Mouse", Price = 150, Category = "tech" };

        _mediator.Send(Arg.Any<GetProductByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(product);

        var result = await _controller.GetById(3);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAt_WhenSuccessful()
    {
        var request = new CreateProductRequest
        {
            Title = "Smartphone",
            Price = 2500,
            Description = "Fast and sleek",
            Category = "tech",
            Image = "smartphone.jpg",
            Rating = new RatingDto { Rate = 4.8, Count = 100 }
        };

        var command = new CreateProductCommand
        {
            Title = request.Title,
            Price = request.Price,
            Description = request.Description,
            Category = request.Category,
            Image = request.Image,
            Rating = request.Rating
        };

        var created = new ProductDto
        {
            Id = 10,
            Title = request.Title,
            Price = request.Price,
            Description = request.Description,
            Category = request.Category,
            Image = request.Image,
            Rating = request.Rating
        };

        _mapper.Map<CreateProductCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>())
            .Returns(created);

        var result = await _controller.Create(request);

        var createdAt = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdAt.Value.Should().BeEquivalentTo(created);
        createdAt.RouteValues!["id"].Should().Be(created.Id);
    }

    [Fact]
    public async Task Update_ShouldReturnOk_WhenSuccessful()
    {
        var request = new UpdateProductRequest
        {
            Id = 5,
            Title = "Monitor",
            Price = 1200,
            Description = "Full HD display",
            Category = "tech",
            Image = "monitor.jpg",
            Rating = new RatingDto { Rate = 4.5, Count = 80 }
        };

        var command = new UpdateProductCommand
        {
            Id = 5,
            Title = request.Title,
            Price = request.Price,
            Description = request.Description,
            Category = request.Category,
            Image = request.Image,
            Rating = request.Rating
        };

        var updated = new ProductDto
        {
            Id = 5,
            Title = request.Title,
            Price = request.Price,
            Description = request.Description,
            Category = request.Category,
            Image = request.Image,
            Rating = request.Rating
        };

        _mapper.Map<UpdateProductCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>())
            .Returns(updated);

        var result = await _controller.Update(5, request);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(updated);
    }

    [Fact]
    public async Task Delete_ShouldReturnOk_WhenSuccessful()
    {
        _mediator.Send(Arg.Any<DeleteProductCommand>(), Arg.Any<CancellationToken>())
            .Returns(new Deleted());

        var result = await _controller.Delete(2);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetAllCategories_ShouldReturnOk_WhenSuccessful()
    {
        var categories = new List<string> { "tech", "fashion", "books" };

        _mediator.Send(Arg.Any<GetAllCategoriesQuery>(), Arg.Any<CancellationToken>())
            .Returns(categories);

        var result = await _controller.GetAllCategories();

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(categories);
    }

    [Fact]
    public async Task GetByCategory_ShouldReturnOk_WhenQueryIsValid()
    {
        var queryOptions = new RequestQueryOptions { Page = 1, Size = 5, Order = "price" };
        var category = "books";

        var response = new PagedResponse<ProductDto>(
            data: new List<ProductDto> { new ProductDto { Id = 1, Title = "Clean Code", Price = 99, Category = "books" } },
            totalItems: 1,
            currentPage: 1,
            totalPages: 1
        );

        _mediator.Send(Arg.Any<GetProductsByCategoryQuery>(), Arg.Any<CancellationToken>())
            .Returns(response);

        var result = await _controller.GetByCategory(category, queryOptions);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(response);
    }
}