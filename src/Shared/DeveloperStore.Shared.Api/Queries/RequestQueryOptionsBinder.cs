using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace DeveloperStore.Shared.Api.Queries;

public class RequestQueryOptionsBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var query = bindingContext.HttpContext.Request.Query;

        var options = new RequestQueryOptions
        {
            Page = int.TryParse(query["_page"], out var page) ? page : 1,
            Size = int.TryParse(query["_size"], out var size) ? size : 10,
            Order = query["_order"].FirstOrDefault()
        };

        options.Filters = query
            .Where(kvp => !kvp.Key.StartsWith("_"))
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.FirstOrDefault() ?? string.Empty
            );

        options.MinValues = query
            .Where(kvp => kvp.Key.StartsWith("_min"))
            .ToDictionary(
                kvp => kvp.Key.Replace("_min", ""),
                kvp => decimal.TryParse(kvp.Value.FirstOrDefault(), NumberStyles.Number, CultureInfo.InvariantCulture, out var result)
                    ? result
                    : 0
            );

        options.MaxValues = query
            .Where(kvp => kvp.Key.StartsWith("_max"))
            .ToDictionary(
                kvp => kvp.Key.Replace("_max", ""),
                kvp => decimal.TryParse(kvp.Value.FirstOrDefault(), NumberStyles.Number, CultureInfo.InvariantCulture, out var result)
                    ? result
                    : 0
            );

        bindingContext.Result = ModelBindingResult.Success(options);
        return Task.CompletedTask;
    }
}