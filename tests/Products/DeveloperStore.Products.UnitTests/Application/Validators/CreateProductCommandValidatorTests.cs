using FluentAssertions;
using DeveloperStore.Products.Application.Commands.CreateProduct;

namespace DeveloperStore.Products.UnitTests.Application.Validators;

public class CreateProductCommandValidatorTests
{
    private readonly CreateProductCommandValidator _validator = new();

    [Fact]
    public void Should_HaveValidationError_WhenTitleIsEmpty()
    {
        var command = new CreateProductCommand
        {
            Title = "",
            Price = 10.0m,
            Description = "Produto válido",
            Category = "Categoria",
            Image = "img.jpg"
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title" && e.ErrorMessage == "Title is required.");
    }

    [Fact]
    public void Should_HaveValidationError_WhenPriceIsZeroOrNegative()
    {
        var command = new CreateProductCommand
        {
            Title = "Produto",
            Price = 0m,
            Description = "Produto inválido",
            Category = "Categoria",
            Image = "img.jpg"
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Price" && e.ErrorMessage == "Price must be greater than zero.");
    }

    [Fact]
    public void Should_PassValidation_WhenCommandIsValid()
    {
        var command = new CreateProductCommand
        {
            Title = "Produto válido",
            Price = 99.99m,
            Description = "Descrição ok",
            Category = "Categoria",
            Image = "img.jpg"
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }
}