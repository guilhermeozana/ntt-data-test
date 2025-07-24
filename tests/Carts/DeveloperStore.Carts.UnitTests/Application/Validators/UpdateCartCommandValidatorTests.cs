using FluentAssertions;
using DeveloperStore.Carts.Application.Commands.UpdateCart;
using DeveloperStore.Carts.Contracts.Dtos;

namespace DeveloperStore.Carts.UnitTests.Application.Validators;

public class UpdateCartCommandValidatorTests
{
    private readonly UpdateCartCommandValidator _validator = new();

    private UpdateCartCommand BuildValidCommand() => new()
    {
        Id = 10,
        UserId = 99,
        Date = "2025-07-23",
        Products = new List<CartItemDto>
        {
            new() { ProductId = 1001, Quantity = 2 },
            new() { ProductId = 1002, Quantity = 1 }
        }
    };

    [Fact]
    public void Should_PassValidation_WhenCommandIsValid()
    {
        var command = BuildValidCommand();

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_HaveError_WhenIdIsMissing()
    {
        var command = BuildValidCommand();
        command.Id = 0;

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Id" && e.ErrorMessage == "Cart ID is required.");
    }

    [Fact]
    public void Should_HaveError_WhenUserIdIsMissing()
    {
        var command = BuildValidCommand();
        command.UserId = 0;

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "UserId" && e.ErrorMessage == "User ID is required.");
    }

    [Fact]
    public void Should_HaveError_WhenDateIsInvalid()
    {
        var command = BuildValidCommand();
        command.Date = "not-a-date";

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Date" && e.ErrorMessage == "Date must be a valid format.");
    }

    [Fact]
    public void Should_HaveError_WhenProductIdIsMissing()
    {
        var command = BuildValidCommand();
        command.Products[0].ProductId = 0;

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.EndsWith("ProductId") && e.ErrorMessage == "Product ID is required.");
    }

    [Fact]
    public void Should_HaveError_WhenQuantityIsZero()
    {
        var command = BuildValidCommand();
        command.Products[1].Quantity = 0;

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.EndsWith("Quantity") && e.ErrorMessage == "Quantity must be greater than zero.");
    }
}