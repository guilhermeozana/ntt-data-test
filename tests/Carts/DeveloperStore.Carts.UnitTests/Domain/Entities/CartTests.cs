using ErrorOr;
using FluentAssertions;
using DeveloperStore.Carts.Domain.Entities;

namespace DeveloperStore.Carts.UnitTests.Domain.Entities;

public class CartTests
{
    [Fact]
    public void Create_ShouldInitializeCartCorrectly()
    {
        var now = DateTime.UtcNow;
        var cart = Cart.Create(userId: 7, date: now);

        cart.UserId.Should().Be(7);
        cart.Date.Should().Be(now);
        cart.Products.Should().BeEmpty();
    }

    [Fact]
    public void Update_ShouldChangeUserIdAndDate()
    {
        var cart = Cart.Create(5);
        var newDate = new DateTime(2025, 10, 1);

        cart.Update(userId: 15, date: newDate);

        cart.UserId.Should().Be(15);
        cart.Date.Should().Be(newDate);
    }

    [Fact]
    public void AddItem_ShouldAddNewCartItem_WhenQuantityIsValid()
    {
        var cart = Cart.Create(1);

        var result = cart.AddItem(productId: 1001, quantity: 3);

        result.IsError.Should().BeFalse();
        cart.Products.Should().ContainSingle(i => i.ProductId == 1001 && i.Quantity == 3);
    }

    [Fact]
    public void AddItem_ShouldReturnValidationError_WhenQuantityExceedsLimit()
    {
        var cart = Cart.Create(2);

        var result = cart.AddItem(productId: 1002, quantity: 21);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Validation);
        result.FirstError.Description.Should().Be("Cannot purchase more than 20 identical items.");
    }

    [Fact]
    public void ClearItems_ShouldRemoveAllCartItems()
    {
        var cart = Cart.Create(3);
        cart.AddItem(2001, 2);
        cart.AddItem(2002, 1);

        cart.Products.Should().HaveCount(2);

        cart.ClearItems();

        cart.Products.Should().BeEmpty();
    }
}