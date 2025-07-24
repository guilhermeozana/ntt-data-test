using FluentAssertions;
using DeveloperStore.Users.Domain.ValueObjects;
using Xunit;

namespace DeveloperStore.Users.UnitTests.Domain.ValueObjects;

public class AddressTests
{
    private Geolocation BuildGeolocation(string lat = "22.9068", string lng = "43.1729")
        => new Geolocation(lat, lng);

    [Fact]
    public void Constructor_ShouldInitializeAllProperties()
    {
        var geo = BuildGeolocation();
        var address = new Address("Rio de Janeiro", "Rua das Flores", 123, "20000-000", geo);

        address.City.Should().Be("Rio de Janeiro");
        address.Street.Should().Be("Rua das Flores");
        address.Number.Should().Be(123);
        address.Zipcode.Should().Be("20000-000");
        address.Geolocation.Should().Be(geo);
    }

    [Fact]
    public void Addresses_WithSameData_ShouldBeEqual()
    {
        var geo = BuildGeolocation();
        var address1 = new Address("RJ", "Rua 1", 10, "11111-000", geo);
        var address2 = new Address("RJ", "Rua 1", 10, "11111-000", geo);

        address1.Should().Be(address2);
    }

    [Fact]
    public void Addresses_WithDifferentCity_ShouldNotBeEqual()
    {
        var geo = BuildGeolocation();
        var address1 = new Address("RJ", "Rua 1", 10, "11111-000", geo);
        var address2 = new Address("SP", "Rua 1", 10, "11111-000", geo);

        address1.Should().NotBe(address2);
    }

    [Fact]
    public void Addresses_WithDifferentGeolocation_ShouldNotBeEqual()
    {
        var address1 = new Address("RJ", "Rua 1", 10, "11111-000", BuildGeolocation("22.90", "43.17"));
        var address2 = new Address("RJ", "Rua 1", 10, "11111-000", BuildGeolocation("23.00", "43.17"));

        address1.Should().NotBe(address2);
    }
}