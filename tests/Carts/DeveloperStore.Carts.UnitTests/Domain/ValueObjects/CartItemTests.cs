using FluentAssertions;
using DeveloperStore.Carts.Domain.ValueObjects;

namespace DeveloperStore.Carts.UnitTests.Domain.ValueObjects;

public class CartItemTests
{
    [Fact]
    public void Should_CreateCartItem_WithCorrectValues()
    {
        var item = new CartItem(productId: 101, quantity: 3);

        item.ProductId.Should().Be(101);
        item.Quantity.Should().Be(3);
    }

    [Fact]
    public void Should_ConsiderTwoCartItemsEqual_WhenPropertiesAreIdentical()
    {
        var item1 = new CartItem(200, 2);
        var item2 = new CartItem(200, 2);

        item1.Should().Be(item2); // Usa lógica de ValueObject
    }

    [Fact]
    public void Should_ConsiderTwoCartItemsDifferent_WhenProductIdOrQuantityDiffer()
    {
        var item1 = new CartItem(300, 1);
        var item2 = new CartItem(300, 5);
        var item3 = new CartItem(999, 1);

        item1.Should().NotBe(item2);
        item1.Should().NotBe(item3);
    }
}