using FluentAssertions;
using DeveloperStore.Sales.Domain.ValueObjects;

namespace DeveloperStore.Sales.UnitTests.Domain.ValueObjects;

public class SaleItemTests
{
    [Theory]
    [InlineData(3, 0.00)]
    [InlineData(4, 0.10)]
    [InlineData(9, 0.10)]
    [InlineData(10, 0.20)]
    [InlineData(15, 0.20)]
    [InlineData(20, 0.20)]
    [InlineData(21, 0.00)]
    public void DiscountRate_ShouldMatchQuantityRange(int quantity, decimal expectedRate)
    {
        var item = new SaleItem(1, quantity, 100);

        item.DiscountRate.Should().Be(expectedRate);
    }

    [Fact]
    public void Should_CalculateDiscountCorrectly()
    {
        var item = new SaleItem(productId: 10, quantity: 5, unitPrice: 100);

        item.DiscountRate.Should().Be(0.10m);
        item.Discount.Should().Be(50);
        item.TotalAmount.Should().Be(450);
    }

    [Fact]
    public void Should_HandleZeroDiscountRate()
    {
        var item = new SaleItem(10, 2, 50);

        item.DiscountRate.Should().Be(0.00m);
        item.Discount.Should().Be(0);
        item.TotalAmount.Should().Be(100);
    }

    [Fact]
    public void Should_RespectEquality_AndBehaveLikeValueObject()
    {
        var item1 = new SaleItem(99, 10, 25);
        var item2 = new SaleItem(99, 10, 25);
        var item3 = new SaleItem(99, 9, 25);

        item1.Should().Be(item2);
        item1.Should().NotBe(item3);
    }
}