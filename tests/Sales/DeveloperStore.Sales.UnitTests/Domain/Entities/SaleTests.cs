using FluentAssertions;
using DeveloperStore.Sales.Domain.Entities;

namespace DeveloperStore.Sales.UnitTests.Domain.Entities;

public class SaleTests
{
    [Fact]
    public void Create_ShouldInitializeSaleAndEmitEvent()
    {
        // Arrange
        var now = DateTime.Today;

        // Act
        var sale = Sale.Create("SALE-0001", now, customerId: 10, branchId: 1);

        // Assert
        sale.SaleNumber.Should().Be("SALE-0001");
        sale.Date.Should().Be(now);
        sale.CustomerId.Should().Be(10);
        sale.BranchId.Should().Be(1);
        sale.Products.Should().BeEmpty();
        sale.IsCancelled.Should().BeFalse();
        sale.DomainEvents.Should().ContainSingle(e => e is DeveloperStore.Sales.Domain.Events.SaleCreatedEvent);
    }

    [Fact]
    public void AddItem_ShouldAddValidItemAndCalculateTotals()
    {
        var sale = Sale.Create("S-1", DateTime.Now, 1, 1);

        var result = sale.AddItem(101, 5, 100);

        result.IsError.Should().BeFalse();
        sale.Products.Should().ContainSingle(i => i.ProductId == 101);
        sale.TotalAmount.Should().Be(450); // 5 * 100 - 10% desconto
    }

    [Fact]
    public void AddItem_ShouldReplaceExistingItemWithSameProductId()
    {
        var sale = Sale.Create("S-2", DateTime.Now, 1, 1);

        sale.AddItem(999, 4, 100); // 10% desconto = 360
        sale.AddItem(999, 10, 100); // 20% desconto = 800

        sale.Products.Should().HaveCount(1);
        sale.TotalAmount.Should().Be(800);
    }

    [Fact]
    public void AddItem_ShouldReturnError_WhenQuantityExceedsLimit()
    {
        var sale = Sale.Create("S-3", DateTime.Now, 1, 1);

        var result = sale.AddItem(999, 25, 50);

        result.IsError.Should().BeTrue();
        result.FirstError.Description.Should().Contain("Cannot purchase more than");
        sale.Products.Should().BeEmpty();
    }

    [Fact]
    public void Update_ShouldChangeValuesAndEmitModifiedEvent()
    {
        var sale = Sale.Create("S-4", DateTime.Today, 1, 1);

        sale.Update(DateTime.Today.AddDays(1), 2, 3);

        sale.CustomerId.Should().Be(2);
        sale.BranchId.Should().Be(3);
        sale.DomainEvents.Should().Contain(e => e is DeveloperStore.Sales.Domain.Events.SaleModifiedEvent);
    }

    [Fact]
    public void Cancel_ShouldMarkSaleAsCancelledAndEmitEvents()
    {
        var sale = Sale.Create("S-5", DateTime.Now, 1, 1);
        sale.AddItem(1001, 5, 100);
        sale.AddItem(1002, 3, 150);

        sale.Cancel();

        sale.IsCancelled.Should().BeTrue();
        sale.DomainEvents.Should().Contain(e => e is DeveloperStore.Sales.Domain.Events.SaleCancelledEvent);
        sale.DomainEvents.Count(e => e is DeveloperStore.Sales.Domain.Events.ItemCancelledEvent).Should().Be(2);
    }

    [Fact]
    public void Cancel_ShouldDoNothing_WhenAlreadyCancelled()
    {
        var sale = Sale.Create("S-6", DateTime.Now, 1, 1);
        sale.Cancel();
        sale.Cancel(); // segunda chamada

        sale.IsCancelled.Should().BeTrue();
        sale.DomainEvents.Count(e => e is DeveloperStore.Sales.Domain.Events.SaleCancelledEvent).Should().Be(1);
    }

    [Fact]
    public void ClearItems_ShouldRemoveAllProducts()
    {
        var sale = Sale.Create("S-7", DateTime.Now, 1, 1);
        sale.AddItem(3001, 5, 10);
        sale.AddItem(3002, 2, 15);

        sale.ClearItems();

        sale.Products.Should().BeEmpty();
        sale.TotalAmount.Should().Be(0);
    }
}