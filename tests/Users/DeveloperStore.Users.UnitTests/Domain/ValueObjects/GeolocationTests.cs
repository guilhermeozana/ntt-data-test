using FluentAssertions;
using DeveloperStore.Users.Domain.ValueObjects;

namespace DeveloperStore.Users.UnitTests.Domain.ValueObjects;

public class GeolocationTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange & Act
        var geo = new Geolocation("22.9068", "43.1729");

        // Assert
        geo.Lat.Should().Be("22.9068");
        geo.Long.Should().Be("43.1729");
    }

    [Fact]
    public void Geolocations_WithSameValues_ShouldBeEqual()
    {
        var geo1 = new Geolocation("22.9068", "43.1729");
        var geo2 = new Geolocation("22.9068", "43.1729");

        geo1.Should().Be(geo2);
    }

    [Fact]
    public void Geolocations_WithDifferentLat_ShouldNotBeEqual()
    {
        var geo1 = new Geolocation("22.9000", "43.1729");
        var geo2 = new Geolocation("22.9068", "43.1729");

        geo1.Should().NotBe(geo2);
    }

    [Fact]
    public void Geolocations_WithDifferentLong_ShouldNotBeEqual()
    {
        var geo1 = new Geolocation("22.9068", "43.0000");
        var geo2 = new Geolocation("22.9068", "43.1729");

        geo1.Should().NotBe(geo2);
    }
}