using Microsoft.AspNetCore.Mvc;

namespace DeveloperStore.Shared.Api.Queries;

public class RequestQueryOptions
{
    [FromQuery(Name = "_page")]
    public int Page { get; set; } = 1;

    [FromQuery(Name = "_size")]
    public int Size { get; set; } = 10;

    [FromQuery(Name = "_order")]
    public string? Order { get; set; }

    [FromQuery]
    public Dictionary<string, string>? Filters { get; set; }

    [FromQuery]
    public Dictionary<string, decimal>? MinValues { get; set; }

    [FromQuery]
    public Dictionary<string, decimal>? MaxValues { get; set; }
}