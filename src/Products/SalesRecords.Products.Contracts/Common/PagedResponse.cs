namespace SalesRecords.Products.Api.Contracts.Common;

public class PagedResponse<T>
{
    public IEnumerable<T> Data { get; set; }
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}