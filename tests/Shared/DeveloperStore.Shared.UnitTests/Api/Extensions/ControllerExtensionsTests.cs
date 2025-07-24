using System.Text.Json;
using DeveloperStore.Shared.Api.Extensions;
using ErrorOr;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.Shared.UnitTests.Api.Extensions;

public class ControllerExtensionsTests
{
    private class TestController : ControllerBase { }

    private readonly TestController _controller = new();

    private static JsonElement ToJsonElement(object value)
    {
        var json = JsonSerializer.Serialize(value);
        return JsonDocument.Parse(json).RootElement;
    }

    [Fact]
    public void Problem_ShouldReturn500_WhenErrorListIsEmpty()
    {
        // Arrange
        var errors = new List<Error>();

        // Act
        var result = _controller.Problem(errors) as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(500);

        var root = ToJsonElement(result.Value!);
        root.GetProperty("type").GetString().Should().Be("InternalServerError");
        root.GetProperty("error").GetString().Should().Be("Unexpected failure");
        root.GetProperty("detail").GetString().Should().Be("No error information was provided");
    }

    [Fact]
    public void Problem_ShouldReturnValidationProblem_WhenAllErrorsAreValidation()
    {
        // Arrange
        var errors = new List<Error>
        {
            Error.Validation("field1", "Invalid name"),
            Error.Validation("field2", "Invalid price")
        };

        // Act
        var result = _controller.Problem(errors) as BadRequestObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(400);

        var root = ToJsonElement(result.Value!);
        root.GetProperty("type").GetString().Should().Be("ValidationError");
        root.GetProperty("error").GetString().Should().Be("Invalid input data");
        root.GetProperty("detail").GetString().Should().Contain("Invalid name");
        root.GetProperty("detail").GetString().Should().Contain("Invalid price");
    }

    [Theory]
    [InlineData(ErrorType.NotFound, 404, "ResourceNotFound", "Product not found")]
    [InlineData(ErrorType.Unauthorized, 401, "AuthenticationError", "Invalid authentication token")]
    [InlineData(ErrorType.Conflict, 409, "ConflictError", "Conflicting operation")]
    [InlineData(ErrorType.Validation, 400, "ValidationError", "Invalid input data")]
    [InlineData(ErrorType.Unexpected, 500, "InternalServerError", "Unexpected failure")]
    public void Problem_ShouldMapErrorTypeToStatusCode(ErrorType type, int expectedCode, string expectedType, string expectedMessage)
    {
        // Arrange
        var error = Error.Custom((int)type, "err_code", "detail message");

        // Act
        var result = _controller.Problem(error) as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(expectedCode);

        var root = ToJsonElement(result.Value!);
        root.GetProperty("type").GetString().Should().Be(expectedType);
        root.GetProperty("error").GetString().Should().Be(expectedMessage);
        root.GetProperty("detail").GetString().Should().Be("detail message");
    }
}