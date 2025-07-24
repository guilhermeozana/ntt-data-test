using FluentAssertions;
using DeveloperStore.Shared.Contracts.Dtos;
using DeveloperStore.Users.Application.Commands.CreateUser;
using Xunit;

namespace DeveloperStore.Users.UnitTests.Application.Validators;

public class CreateUserCommandValidatorTests
{
    private readonly CreateUserCommandValidator _validator = new();

    private CreateUserCommand BuildValidCommand() =>
        new CreateUserCommand
        {
            Email = "user@email.com",
            Username = "username123",
            Password = "password!",
            Phone = "21-99999-9999",
            Status = "Active",
            Role = "Admin",
            Name = new NameDto
            {
                Firstname = "Ana",
                Lastname = "Lima"
            },
            Address = new AddressDto
            {
                City = "Rio",
                Street = "Rua 1",
                Number = 123,
                Zipcode = "20000-000",
                Geolocation = new GeolocationDto
                {
                    Lat = "-22.9",
                    Long = "-43.1"
                }
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
    public void Should_Fail_WhenEmailIsInvalid()
    {
        var command = BuildValidCommand();
        command.Email = "invalid-email";

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Should_Fail_WhenEnumValuesAreInvalid()
    {
        var command = BuildValidCommand();
        command.Status = "UnknownStatus";
        command.Role = "UnknownRole";

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Status");
        result.Errors.Should().Contain(e => e.PropertyName == "Role");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Fail_WhenNameIsMissing(string value)
    {
        var command = BuildValidCommand();
        command.Name.Firstname = value;
        command.Name.Lastname = value;

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name.Firstname");
        result.Errors.Should().Contain(e => e.PropertyName == "Name.Lastname");
    }

    [Theory]
    [InlineData("", "Street", 10)]
    [InlineData("City", "", 10)]
    [InlineData("City", "Street", 0)]
    public void Should_Fail_WhenAddressIsInvalid(string city, string street, int number)
    {
        var command = BuildValidCommand();
        command.Address.City = city;
        command.Address.Street = street;
        command.Address.Number = number;

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Should_Fail_WhenGeolocationIsMissing()
    {
        var command = BuildValidCommand();
        command.Address.Geolocation.Lat = "";
        command.Address.Geolocation.Long = "";

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Address.Geolocation.Lat");
        result.Errors.Should().Contain(e => e.PropertyName == "Address.Geolocation.Long");
    }
}