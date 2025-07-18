using SalesRecords.Products.Api.Contracts.Common;

namespace SalesRecords.Products.Api.Contracts;

public class GetProductsByCategoryRequest : PaginationQuery
{
    public string Category { get; set; }
    public string _order { get; set; }
}