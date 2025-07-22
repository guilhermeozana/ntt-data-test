using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace SalesRecords.Shared.Api.Extensions;

public static class ControllerExtensions
{
    public static IActionResult Problem(this ControllerBase controller, List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return controller.StatusCode(500, new
            {
                type = "InternalServerError",
                error = "Unexpected failure",
                detail = "No error information was provided"
            });
        }

        if (errors.All(e => e.Type == ErrorType.Validation))
        {
            return controller.ValidationProblem(errors);
        }

        return controller.Problem(errors[0]);
    }

    public static IActionResult ValidationProblem(this ControllerBase controller, List<Error> errors)
    {
        var details = errors.Select(e => e.Description).ToList();

        return controller.BadRequest(new
        {
            type = "ValidationError",
            error = "Invalid input data",
            detail = string.Join(" | ", details)
        });
    }

    public static IActionResult Problem(this ControllerBase controller, Error error)
    {
        var (statusCode, type, summary) = error.Type switch
        {
            ErrorType.NotFound => (404, "ResourceNotFound", "Product not found"),
            ErrorType.Unauthorized => (401, "AuthenticationError", "Invalid authentication token"),
            ErrorType.Conflict => (409, "ConflictError", "Conflicting operation"),
            ErrorType.Validation => (400, "ValidationError", "Invalid input data"),
            _ => (500, "InternalServerError", "Unexpected failure")
        };

        return controller.StatusCode(statusCode, new
        {
            type = type,
            error = summary,
            detail = error.Description
        });
    }
}