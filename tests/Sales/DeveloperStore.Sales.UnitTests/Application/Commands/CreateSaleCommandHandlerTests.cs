using FluentAssertions;
using NSubstitute;
using DeveloperStore.Sales.Application.Commands.CreateSale;
using DeveloperStore.Sales.Application.Common.Events;
using DeveloperStore.Sales.Application.Common.Repositories;
using DeveloperStore.Sales.Contracts.Dtos;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Domain.ValueObjects;
using Xunit;
using AutoMapper;
using ErrorOr;
using DeveloperStore.Shared.SharedKernel.Domain.Events;

namespace DeveloperStore.Sales.UnitTests.Application.Commands;

public class CreateSaleCommandHandlerTests
{
    private readonly ISaleRepository _repo = Substitute.For<ISaleRepository>();
    private readonly IDomainEventDispatcher _dispatcher = Substitute.For<IDomainEventDispatcher>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    private CreateSaleCommandHandler CreateHandler() => new(_repo, _mapper, _dispatcher);

    [Fact]
    public async Task Handle_ShouldReturnSaleDto_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            Date = DateTime.Today.ToString("yyyy-MM-dd"),
            CustomerId = 1,
            BranchId = 2,
            Products = new List<SaleItemDto>
            {
                new() { ProductId = 10, Quantity = 5, UnitPrice = 100 },
                new() { ProductId = 11, Quantity = 3, UnitPrice = 200 }
            }
        };

        var handler = CreateHandler();

        _mapper.Map<SaleDto>(Arg.Any<Sale>()).Returns(new SaleDto { SaleNumber = "MockedSaleNumber" });

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.SaleNumber.Should().Be("MockedSaleNumber");

        await _repo.Received(1).AddAsync(Arg.Any<Sale>());
        await _dispatcher.Received(1).DispatchEventsAsync(Arg.Any<IEnumerable<IDomainEvent>>());
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenItemIsInvalid()
    {
        var command = new CreateSaleCommand
        {
            Date = DateTime.Today.ToString("yyyy-MM-dd"),
            CustomerId = 1,
            BranchId = 2,
            Products = new List<SaleItemDto>
            {
                new() { ProductId = 10, Quantity = 25, UnitPrice = 100 }
            }
        };

        var handler = CreateHandler();

        var result = await handler.Handle(command, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task Handle_ShouldClearEvents_AfterDispatch()
    {
        var command = new CreateSaleCommand
        {
            Date = DateTime.Today.ToString("yyyy-MM-dd"),
            CustomerId = 1,
            BranchId = 1,
            Products = new List<SaleItemDto>
            {
                new() { ProductId = 10, Quantity = 5, UnitPrice = 100 }
            }
        };

        var handler = CreateHandler();

        Sale capturedSale = null!;
        await _repo.AddAsync(Arg.Do<Sale>(s => capturedSale = s));

        _mapper.Map<SaleDto>(Arg.Any<Sale>()).Returns(new SaleDto());

        await handler.Handle(command, default);

        capturedSale.DomainEvents.Should().BeEmpty();
    }
}