using FluentAssertions;
using DeveloperStore.Sales.Application.Commands.UpdateSale;
using DeveloperStore.Sales.Contracts.Dtos;
using Xunit;

namespace DeveloperStore.Sales.UnitTests.Application.Validators;

public class UpdateSaleCommandValidatorTests
{
    private readonly UpdateSaleCommandValidator _validator = new();

    private UpdateSaleCommand BuildValidCommand() =>
        new UpdateSaleCommand
        {
            Id = 1,
            CustomerId = 10,
            BranchId = 20,
            Date = DateTime.Today.ToString("yyyy-MM-dd"),
            Products = new List<SaleItemDto>
            {
                new() { ProductId = 101, Quantity = 5, UnitPrice = 50 }
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
    public void Should_FailValidation_WhenIdIsZero()
    {
        var command = BuildValidCommand();
        command.Id = 0;

        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Id");
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
    public void Should_FailValidation_WhenProductQuantityIsTooHigh()
    {
        var command = BuildValidCommand();
        command.Products[0].Quantity = 25;

        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Products[0].Quantity"));
    }

    [Fact]
    public void Should_FailValidation_WhenProductsAreEmpty()
    {
        var command = BuildValidCommand();
        command.Products = new List<SaleItemDto>();

        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Products");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Should_Fail_WhenCustomerOrBranchIdIsInvalid(int invalidValue)
    {
        var command = BuildValidCommand();
        command.CustomerId = invalidValue;
        command.BranchId = invalidValue;

        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CustomerId");
        result.Errors.Should().Contain(e => e.PropertyName == "BranchId");
    }
}