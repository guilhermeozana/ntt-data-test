using FluentAssertions;
using DeveloperStore.Users.Domain.Entities;
using DeveloperStore.Users.Domain.Enums;
using DeveloperStore.Users.Domain.ValueObjects;

namespace DeveloperStore.Users.UnitTests.Domain.Entities;

public class UserTests
{
    private Name BuildName() => new("Fulano", "de Tal");

    private Address BuildAddress() =>
        new(
            city: "Rio de Janeiro",
            street: "Rua das Flores",
            number: 123,
            zipcode: "20000-000",
            geolocation: new Geolocation("22.9068", "43.1729")
        );

    [Fact]
    public void Create_ShouldInitializeUserWithCorrectValues()
    {
        // Arrange
        var name = BuildName();
        var address = BuildAddress();

        // Act
        var user = User.Create(
            email: "guilherme@email.com",
            username: "gui_lima",
            password: "12345678",
            name: name,
            address: address,
            phone: "+55 (21) 99999-9999",
            status: UserStatus.Active,
            role: UserRole.Admin
        );

        // Assert
        user.Email.Should().Be("guilherme@email.com");
        user.Username.Should().Be("gui_lima");
        user.Password.Should().Be("12345678");
        user.Name.Should().Be(name);
        user.Address.Should().Be(address);
        user.Phone.Should().Be("+55 (21) 99999-9999");
        user.Status.Should().Be(UserStatus.Active);
        user.Role.Should().Be(UserRole.Admin);
    }

    [Theory]
    [InlineData(UserStatus.Active)]
    [InlineData(UserStatus.Inactive)]
    [InlineData(UserStatus.Suspended)]
    public void Update_ShouldChangeUserStatusToAnyValidEnum(UserStatus newStatus)
    {
        // Arrange
        var user = User.Create(
            email: "original@email.com",
            username: "orig_user",
            password: "password123",
            name: BuildName(),
            address: BuildAddress(),
            phone: "1234",
            status: UserStatus.Active,
            role: UserRole.Admin
        );

        var newName = new Name("Lucas", "Silva");
        var newAddress = new Address(
            city: "São Paulo",
            street: "Av. Central",
            number: 500,
            zipcode: "01000-000",
            geolocation: new Geolocation("23.5505", "46.6333")
        );

        // Act
        user.Update(
            email: "lucas@email.com",
            username: "lucas.silva",
            password: "novaSenha@2025",
            name: newName,
            address: newAddress,
            phone: "(11) 91234-5678",
            status: newStatus,
            role: UserRole.Manager
        );

        // Assert
        user.Email.Should().Be("lucas@email.com");
        user.Username.Should().Be("lucas.silva");
        user.Password.Should().Be("novaSenha@2025");
        user.Name.Should().Be(newName);
        user.Address.Should().Be(newAddress);
        user.Phone.Should().Be("(11) 91234-5678");
        user.Status.Should().Be(newStatus);
        user.Role.Should().Be(UserRole.Manager);
    }
}