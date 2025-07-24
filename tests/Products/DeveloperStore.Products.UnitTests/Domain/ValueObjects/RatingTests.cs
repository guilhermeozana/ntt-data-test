using FluentAssertions;
using DeveloperStore.Products.Domain.ValueObjects;

namespace DeveloperStore.Products.UnitTests.Domain.ValueObjects;

public class RatingTests
{
    [Fact]
    public void Constructor_ShouldInitializeRatingCorrectly()
    {
        var rating = new Rating(rate: 4.7, count: 123);

        rating.Rate.Should().Be(4.7);
        rating.Count.Should().Be(123);
    }

    [Fact]
    public void Properties_ShouldBeMutable()
    {
        var rating = new Rating(rate: 3.5, count: 80);

        rating.Rate = 4.9;
        rating.Count = 150;

        rating.Rate.Should().Be(4.9);
        rating.Count.Should().Be(150);
    }

    [Theory]
    [InlineData(0.0, 0)]
    [InlineData(5.0, 9999)]
    public void Constructor_ShouldAcceptBoundaryValues(double rate, int count)
    {
        var rating = new Rating(rate, count);

        rating.Rate.Should().Be(rate);
        rating.Count.Should().Be(count);
    }
}