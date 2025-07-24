using FluentAssertions;
using DeveloperStore.Sales.Application.Commands.CreateSale;
using DeveloperStore.Sales.Contracts.Dtos;

namespace DeveloperStore.Sales.UnitTests.Application.Validators;

public class CreateSaleCommandValidatorTests
{
    private readonly CreateSaleCommandValidator _validator = new();

    private CreateSaleCommand BuildValidCommand() =>
        new CreateSaleCommand
        {
            Date = DateTime.Today.ToString("yyyy-MM-dd"),
            CustomerId = 1,
            BranchId = 1,
            Products = new List<SaleItemDto>
            {
                new() { ProductId = 100, Quantity = 5, UnitPrice = 100 }
            }
        };

    [Fact]
    public void Should_ValidateSuccessfully_WhenCommandIsValid()
    {
        var command = BuildValidCommand();
        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_FailValidation_WhenCustomerIdIsZero()
    {
        var command = BuildValidCommand();
        command.CustomerId = 0;

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CustomerId");
    }

    [Fact]
    public void Should_FailValidation_WhenDateIsInvalid()
    {
        var command = BuildValidCommand();
        command.Date = "not-a-date";

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Date");
    }

    [Fact]
    public void Should_Fail_WhenProductQuantityIsTooHigh()
    {
        var command = BuildValidCommand();
        command.Products[0].Quantity = 25;

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Products[0].Quantity"));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Should_ValidateQuantityDiscountRule(int quantity)
    {
        var command = BuildValidCommand();
        command.Products[0].Quantity = quantity;

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_WhenProductsAreEmpty()
    {
        var command = BuildValidCommand();
        command.Products = new List<SaleItemDto>();

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Products");
    }
}