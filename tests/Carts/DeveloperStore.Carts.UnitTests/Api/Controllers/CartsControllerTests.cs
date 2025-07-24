using AutoMapper;
using DeveloperStore.Carts.Api.Controllers;
using DeveloperStore.Carts.Application.Commands.CreateCart;
using DeveloperStore.Carts.Application.Commands.DeleteCart;
using DeveloperStore.Carts.Application.Commands.UpdateCart;
using DeveloperStore.Carts.Application.Queries.GetAllCarts;
using DeveloperStore.Carts.Application.Queries.GetCartById;
using DeveloperStore.Carts.Contracts.Dtos;
using DeveloperStore.Carts.Contracts.Requests;
using DeveloperStore.Shared.Api.Queries;
using DeveloperStore.Shared.SharedKernel.Responses;
using ErrorOr;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace DeveloperStore.Carts.UnitTests.Api.Controllers;

public class CartsControllerTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly CartsController _controller;

    public CartsControllerTests()
    {
        _controller = new CartsController(_mediator, _mapper);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WhenQueryIsValid()
    {
        var request = new RequestQueryOptions
        {
            Page = 1,
            Size = 10,
            Order = "id"
        };

        var expected = new PagedResponse<CartDto>(
            data: new List<CartDto> { new CartDto { Id = 1, UserId = 10, Date = "2023-01-01" } },
            totalItems: 1,
            currentPage: 1,
            totalPages: 1
        );

        _mediator.Send(Arg.Any<GetAllCartsQuery>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        var result = await _controller.GetAll(request);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenCartExists()
    {
        var cart = new CartDto { Id = 42, UserId = 20, Date = "2023-04-15" };

        _mediator.Send(Arg.Any<GetCartByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(cart);

        var result = await _controller.GetById(42);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(cart);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAt_WhenSuccessful()
    {
        var request = new CreateCartRequest
        {
            Id = 0,
            UserId = 5,
            Date = "2023-06-01",
            Products = new List<CartItemDto> { new CartItemDto { ProductId = 1, Quantity = 2 } }
        };

        var command = new CreateCartCommand
        {
            UserId = request.UserId,
            Date = request.Date,
            Products = request.Products
        };

        var createdCart = new CartDto
        {
            Id = 99,
            UserId = 5,
            Date = "2023-06-01",
            Products = request.Products
        };

        _mapper.Map<CreateCartCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>())
            .Returns(createdCart);

        var result = await _controller.Create(request);

        var created = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        created.Value.Should().BeEquivalentTo(createdCart);
        created.RouteValues!["id"].Should().Be(createdCart.Id);
    }

    [Fact]
    public async Task Update_ShouldReturnOk_WhenUpdateIsSuccessful()
    {
        var request = new UpdateCartRequest
        {
            Date = "2023-07-10",
            Products = new List<CartItemDto> { new CartItemDto { ProductId = 2, Quantity = 1 } }
        };

        var command = new UpdateCartCommand
        {
            Id = 7,
            Date = "2023-07-10",
            Products = request.Products
        };

        var updatedCart = new CartDto
        {
            Id = 7,
            UserId = 8,
            Date = "2023-07-10",
            Products = request.Products
        };

        _mapper.Map<UpdateCartCommand>(request).Returns(command);
        _mediator.Send(Arg.Is<UpdateCartCommand>(c => c.Id == 7), Arg.Any<CancellationToken>())
            .Returns(updatedCart);

        var result = await _controller.Update(7, request);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(updatedCart);
    }

    [Fact]
    public async Task Delete_ShouldReturnOk_WhenDeletedSuccessfully()
    {
        _mediator.Send(Arg.Any<DeleteCartCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result.Deleted);

        var result = await _controller.Delete(3);

        result.Should().BeOfType<OkObjectResult>();
    }
}