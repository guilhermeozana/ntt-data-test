namespace DeveloperStore.Shared.SharedKernel.Dtos;

public class QueryCriteriaDto
{
    public QueryCriteriaDto(int page, int size, string? order, Dictionary<string, string>? filters, Dictionary<string, decimal>? minValues, Dictionary<string, decimal>? maxValues)
    {
        Page = page;
        Size = size;
        Order = order;
        Filters = filters;
        MinValues = minValues;
        MaxValues = maxValues;
    }

    public int Page { get; }
    public int Size { get; }
    public string? Order { get; }
    public Dictionary<string, string>? Filters { get; }
    public Dictionary<string, decimal>? MinValues { get; }
    public Dictionary<string, decimal>? MaxValues { get; }
}