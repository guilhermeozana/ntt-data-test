namespace SalesRecords.Products.Api.Contracts.Common;

public abstract class PaginationQuery
{
    public int _page { get; set; } = 1;
    public int _size { get; set; } = 10;
}
