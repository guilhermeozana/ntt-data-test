using FluentAssertions;
using DeveloperStore.Products.Application.Commands.UpdateProduct;
using DeveloperStore.Products.Contracts.Dtos;

namespace DeveloperStore.Products.UnitTests.Application.Validators;

public class UpdateProductCommandValidatorTests
{
    private readonly UpdateProductCommandValidator _validator = new();

    [Fact]
    public void Should_HaveValidationError_WhenIdIsZero()
    {
        var command = new UpdateProductCommand
        {
            Id = 0,
            Title = "Valid Title",
            Price = 99.99m,
            Description = "Produto válido",
            Category = "Categoria",
            Image = "img.jpg",
            Rating = new RatingDto { Rate = 4.5, Count = 100 }
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Id" &&
            e.ErrorMessage == "Id must be a positive number.");
    }

    [Fact]
    public void Should_HaveValidationError_WhenTitleIsEmpty()
    {
        var command = new UpdateProductCommand
        {
            Id = 1,
            Title = "",
            Price = 99.99m,
            Description = "Produto sem título",
            Category = "Categoria",
            Image = "img.jpg",
            Rating = new RatingDto { Rate = 4.3, Count = 90 }
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Title" &&
            e.ErrorMessage == "Title is required.");
    }

    [Fact]
    public void Should_HaveValidationError_WhenPriceIsZero()
    {
        var command = new UpdateProductCommand
        {
            Id = 1,
            Title = "Produto",
            Price = 0,
            Description = "Valor inválido",
            Category = "Categoria",
            Image = "img.jpg",
            Rating = new RatingDto { Rate = 4.3, Count = 90 }
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Price" &&
            e.ErrorMessage == "Price must be greater than zero.");
    }

    [Fact]
    public void Should_PassValidation_WhenCommandIsValid()
    {
        var command = new UpdateProductCommand
        {
            Id = 1,
            Title = "Produto válido",
            Price = 99.99m,
            Description = "Descrição ok",
            Category = "Categoria",
            Image = "img.jpg",
            Rating = new RatingDto { Rate = 5.0, Count = 999 }
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }
}