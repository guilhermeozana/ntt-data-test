using FluentAssertions;
using DeveloperStore.Users.Domain.ValueObjects;

namespace DeveloperStore.Users.UnitTests.Domain.ValueObjects;

public class NameTests
{
    [Fact]
    public void Constructor_ShouldInitializeAllProperties()
    {
        var name = new Name("Guilherme", "Lima");

        name.Firstname.Should().Be("Guilherme");
        name.Lastname.Should().Be("Lima");
    }

    [Fact]
    public void Names_WithSameValues_ShouldBeEqual()
    {
        var name1 = new Name("Ana", "Beatriz");
        var name2 = new Name("Ana", "Beatriz");

        name1.Should().Be(name2);
    }

    [Fact]
    public void Names_WithDifferentFirstnames_ShouldNotBeEqual()
    {
        var name1 = new Name("João", "Silva");
        var name2 = new Name("Maria", "Silva");

        name1.Should().NotBe(name2);
    }

    [Fact]
    public void Names_WithDifferentLastnames_ShouldNotBeEqual()
    {
        var name1 = new Name("João", "Silva");
        var name2 = new Name("João", "Oliveira");

        name1.Should().NotBe(name2);
    }
}