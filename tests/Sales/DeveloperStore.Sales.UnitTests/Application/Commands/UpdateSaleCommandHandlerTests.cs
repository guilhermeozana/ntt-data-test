using AutoMapper;
using ErrorOr;
using FluentAssertions;
using NSubstitute;
using DeveloperStore.Sales.Application.Commands.UpdateSale;
using DeveloperStore.Sales.Application.Common.Events;
using DeveloperStore.Sales.Application.Common.Repositories;
using DeveloperStore.Sales.Contracts.Dtos;
using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.UnitTests.Application.Commands;

public class UpdateSaleCommandHandlerTests
{
    private readonly ISaleRepository _repo = Substitute.For<ISaleRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IDomainEventDispatcher _dispatcher = Substitute.For<IDomainEventDispatcher>();

    private UpdateSaleCommandHandler CreateHandler() => new(_repo, _mapper, _dispatcher);

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenSaleDoesNotExist()
    {
        var command = new UpdateSaleCommand
        {
            Id = 99,
            Date = "2024-05-05",
            CustomerId = 1,
            BranchId = 2,
            Products = new List<SaleItemDto>()
        };

        _repo.GetByIdAsync(command.Id).Returns((Sale?)null);

        var handler = CreateHandler();
        var result = await handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Code.Should().Be("Sale.NotFound");
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenAnyItemIsInvalid()
    {
        var sale = Sale.Create("S-100", DateTime.Now, 1, 1);

        var command = new UpdateSaleCommand
        {
            Id = sale.Id,
            Date = DateTime.Today.ToString("yyyy-MM-dd"),
            CustomerId = 10,
            BranchId = 20,
            Products = new List<SaleItemDto>
            {
                new() { ProductId = 101, Quantity = 25, UnitPrice = 100 }
            }
        };

        _repo.GetByIdAsync(command.Id).Returns(sale);

        var handler = CreateHandler();
        var result = await handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task Handle_ShouldUpdateSaleAndReturnDto_WhenValid()
    {
        var sale = Sale.Create("S-200", DateTime.Today, 1, 1);

        var command = new UpdateSaleCommand
        {
            Id = sale.Id,
            Date = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd"),
            CustomerId = 2,
            BranchId = 3,
            Products = new List<SaleItemDto>
            {
                new() { ProductId = 1001, Quantity = 5, UnitPrice = 50 },
                new() { ProductId = 1002, Quantity = 3, UnitPrice = 30 }
            }
        };

        _repo.GetByIdAsync(command.Id).Returns(sale);
        _mapper.Map<SaleDto>(Arg.Any<Sale>()).Returns(new SaleDto { SaleNumber = "UPDATED" });

        var handler = CreateHandler();
        var result = await handler.Handle(command, default);

        result.IsError.Should().BeFalse();
        result.Value.SaleNumber.Should().Be("UPDATED");

        await _repo.Received(1).UpdateAsync(sale);
        await _dispatcher.Received(1).DispatchEventsAsync(sale.DomainEvents);
        sale.DomainEvents.Should().BeEmpty();
        sale.Products.Should().HaveCount(2);
        sale.IsCancelled.Should().BeFalse();
    }
}