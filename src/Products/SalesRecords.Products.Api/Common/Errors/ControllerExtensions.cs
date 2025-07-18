using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SalesRecords.Products.Api.Common.Errors;

public static class ControllerExtensions
{
    public static IActionResult Problem(this ControllerBase controller, List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return controller.Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return controller.ValidationProblem(errors);
        }

        return controller.Problem(errors[0]);
    }

    public static IActionResult ValidationProblem(this ControllerBase controller, List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();

        foreach (var error in errors)
        {
            modelStateDictionary.AddModelError(error.Code, error.Description);
        }

        return controller.ValidationProblem(modelStateDictionary);
    }

    public static IActionResult Problem(this ControllerBase controller, Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError,
        };

        return controller.Problem(
            statusCode: statusCode,
            detail: error.Description);
    }
}