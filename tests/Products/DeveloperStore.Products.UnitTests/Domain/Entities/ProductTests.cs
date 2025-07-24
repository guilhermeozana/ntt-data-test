using FluentAssertions;
using DeveloperStore.Products.Domain.Entities;
using DeveloperStore.Products.Domain.ValueObjects;

namespace DeveloperStore.Products.UnitTests.Domain.Entities;

public class ProductTests
{
    private Rating BuildRating(double rate = 4.5, int count = 50)
        => new(rate, count);

    [Fact]
    public void Create_ShouldInitializeProductWithCorrectValues()
    {
        var rating = BuildRating();

        var product = Product.Create(
            title: "Notebook Dell",
            price: 3500.99m,
            description: "Notebook potente para desenvolvimento",
            category: "Eletrônicos",
            image: "image-url",
            rating: rating
        );

        product.Title.Should().Be("Notebook Dell");
        product.Price.Should().Be(3500.99m);
        product.Description.Should().Be("Notebook potente para desenvolvimento");
        product.Category.Should().Be("Eletrônicos");
        product.Image.Should().Be("image-url");
        product.Rating.Should().Be(rating);
    }

    [Fact]
    public void Update_ShouldChangeProductProperties()
    {
        var product = Product.Create(
            title: "Notebook Dell",
            price: 3500.99m,
            description: "Notebook potente para desenvolvimento",
            category: "Eletrônicos",
            image: "image-url",
            rating: BuildRating()
        );

        var updatedRating = BuildRating(rate: 4.8, count: 120);

        // Act
        product.Update(
            title: "Notebook HP",
            price: 2999.99m,
            description: "Modelo atualizado com ótimo custo-benefício",
            category: "Tecnologia",
            image: "new-image-url",
            rating: updatedRating
        );

        // Assert
        product.Title.Should().Be("Notebook HP");
        product.Price.Should().Be(2999.99m);
        product.Description.Should().Be("Modelo atualizado com ótimo custo-benefício");
        product.Category.Should().Be("Tecnologia");
        product.Image.Should().Be("new-image-url");
        product.Rating.Should().Be(updatedRating);
    }
}