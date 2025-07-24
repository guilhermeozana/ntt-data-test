using FluentAssertions;
using DeveloperStore.Auth.Application.Commands.Login;

namespace DeveloperStore.Auth.UnitTests.Application.Validators;

public class LoginCommandValidatorTests
{
    private readonly LoginCommandValidator _validator = new();

    [Fact]
    public void Should_PassValidation_WhenUsernameAndPasswordAreValid()
    {
        var command = new LoginCommand("john.doe", "securePass123");

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_HaveError_WhenUsernameIsEmpty()
    {
        var command = new LoginCommand("", "securePass123");

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => 
            e.PropertyName == "Username" && 
            e.ErrorMessage == "Username is required.");
    }

    [Fact]
    public void Should_HaveError_WhenUsernameIsTooShort()
    {
        var command = new LoginCommand("jo", "securePass123");

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => 
            e.PropertyName == "Username" && 
            e.ErrorMessage == "Username must be at least 3 characters long.");
    }

    [Fact]
    public void Should_HaveError_WhenPasswordIsEmpty()
    {
        var command = new LoginCommand("john.doe", "");

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => 
            e.PropertyName == "Password" && 
            e.ErrorMessage == "Password is required.");
    }

    [Fact]
    public void Should_HaveError_WhenPasswordIsTooShort()
    {
        var command = new LoginCommand("john.doe", "12345");

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => 
            e.PropertyName == "Password" && 
            e.ErrorMessage == "Password must be at least 6 characters long.");
    }
}