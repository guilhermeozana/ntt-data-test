using ErrorOr;
using FluentAssertions;
using NSubstitute;
using DeveloperStore.Sales.Application.Commands.CancelSale;
using DeveloperStore.Sales.Application.Common.Events;
using DeveloperStore.Sales.Application.Common.Repositories;
using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.UnitTests.Application.Commands;

public class CancelSaleCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenSaleDoesNotExist()
    {
        // Arrange
        var repo = Substitute.For<ISaleRepository>();
        var dispatcher = Substitute.For<IDomainEventDispatcher>();
        var handler = new CancelSaleCommandHandler(repo, dispatcher);

        var command = new CancelSaleCommand(99);

        repo.GetByIdAsync(command.Id).Returns((Sale?)null);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        result.FirstError.Code.Should().Be("Sale.NotFound");
    }

    [Fact]
    public async Task Handle_ShouldCancelSaleAndPersist_WhenSaleExists()
    {
        // Arrange
        var repo = Substitute.For<ISaleRepository>();
        var dispatcher = Substitute.For<IDomainEventDispatcher>();
        var handler = new CancelSaleCommandHandler(repo, dispatcher);

        var sale = Sale.Create("S-001", DateTime.Today, 1, 1);
        sale.AddItem(101, 5, 100);
        var command = new CancelSaleCommand(sale.Id);

        repo.GetByIdAsync(command.Id).Returns(sale);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsError.Should().BeFalse();
        sale.IsCancelled.Should().BeTrue();

        await repo.Received(1).UpdateAsync(sale);
        await dispatcher.Received(1).DispatchEventsAsync(sale.DomainEvents);
    }

    [Fact]
    public async Task Handle_ShouldClearDomainEvents_AfterDispatching()
    {
        // Arrange
        var repo = Substitute.For<ISaleRepository>();
        var dispatcher = Substitute.For<IDomainEventDispatcher>();
        var handler = new CancelSaleCommandHandler(repo, dispatcher);

        var sale = Sale.Create("S-002", DateTime.Today, 1, 1);
        sale.AddItem(101, 5, 100);
        sale.Cancel();
        var command = new CancelSaleCommand(sale.Id);

        repo.GetByIdAsync(command.Id).Returns(sale);

        // Act
        await handler.Handle(command, default);

        // Assert
        sale.DomainEvents.Should().BeEmpty();
    }
}