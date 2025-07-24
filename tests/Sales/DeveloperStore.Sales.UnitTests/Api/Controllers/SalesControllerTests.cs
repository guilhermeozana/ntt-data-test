using AutoMapper;
using DeveloperStore.Sales.Api.Controllers;
using DeveloperStore.Sales.Application.Commands.CancelSale;
using DeveloperStore.Sales.Application.Commands.CreateSale;
using DeveloperStore.Sales.Application.Commands.UpdateSale;
using DeveloperStore.Sales.Application.Queries.GetAllSales;
using DeveloperStore.Sales.Application.Queries.GetSaleById;
using DeveloperStore.Sales.Contracts.Dtos;
using DeveloperStore.Sales.Contracts.Requests;
using DeveloperStore.Shared.Api.Queries;
using DeveloperStore.Shared.SharedKernel.Responses;
using ErrorOr;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace DeveloperStore.Sales.UnitTests.Api.Controllers;

public class SalesControllerTests
{
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly SalesController _controller;

    public SalesControllerTests()
    {
        _controller = new SalesController(_mediator, _mapper);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WhenQueryIsValid()
    {
        var request = new RequestQueryOptions { Page = 1, Size = 10, Order = "date" };

        var expected = new PagedResponse<SaleDto>(
            data: new List<SaleDto> { new SaleDto { Id = 1, CustomerId = 20, BranchId = 2, Date = "2024-05-10" } },
            totalItems: 1,
            currentPage: 1,
            totalPages: 1
        );

        _mediator.Send(Arg.Any<GetAllSalesQuery>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        var result = await _controller.GetAll(request);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenSaleExists()
    {
        var sale = new SaleDto
        {
            Id = 42,
            CustomerId = 5,
            BranchId = 1,
            Date = "2024-01-15",
            Products = new List<SaleItemDto> {
                new SaleItemDto { ProductId = 1, Quantity = 2, UnitPrice = 99 }
            }
        };

        _mediator.Send(Arg.Any<GetSaleByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        var result = await _controller.GetById(42);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(sale);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAt_WhenSuccessful()
    {
        var request = new CreateSaleRequest
        {
            CustomerId = 5,
            BranchId = 3,
            Date = "2024-04-01",
            Products = new List<SaleItemDto> {
                new SaleItemDto { ProductId = 1, Quantity = 2, UnitPrice = 100 }
            }
        };

        var command = new CreateSaleCommand
        {
            CustomerId = request.CustomerId,
            BranchId = request.BranchId,
            Date = request.Date,
            Products = request.Products
        };

        var createdSale = new SaleDto
        {
            Id = 99,
            CustomerId = request.CustomerId,
            BranchId = request.BranchId,
            Date = request.Date,
            Products = new List<SaleItemDto> {
                new SaleItemDto { ProductId = 1, Quantity = 2, UnitPrice = 100 }
            }
        };

        _mapper.Map<CreateSaleCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>())
            .Returns(createdSale);

        var result = await _controller.Create(request);

        var created = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        created.Value.Should().BeEquivalentTo(createdSale);
        created.RouteValues!["id"].Should().Be(createdSale.Id);
    }

    [Fact]
    public async Task Update_ShouldReturnOk_WhenSuccessful()
    {
        var request = new UpdateSaleRequest
        {
            CustomerId = 8,
            BranchId = 2,
            UserId = 15,
            Date = "2024-06-10",
            Products = new List<SaleItemDto> {
                new SaleItemDto { ProductId = 2, Quantity = 1, UnitPrice = 120 }
            }
        };

        var command = new UpdateSaleCommand
        {
            Id = 7,
            CustomerId = request.CustomerId,
            BranchId = request.BranchId,
            Date = request.Date,
            Products = request.Products
        };

        var updatedSale = new SaleDto
        {
            Id = 7,
            CustomerId = request.CustomerId,
            BranchId = request.BranchId,
            Date = request.Date,
            Products = request.Products
        };

        _mapper.Map<UpdateSaleCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>())
            .Returns(updatedSale);

        var result = await _controller.Update(7, request);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(updatedSale);
    }

    [Fact]
    public async Task Cancel_ShouldReturnOk_WhenSuccessful()
    {
        _mediator.Send(Arg.Any<CancelSaleCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success);

        var result = await _controller.Cancel(3);

        result.Should().BeOfType<OkObjectResult>();
    }
}