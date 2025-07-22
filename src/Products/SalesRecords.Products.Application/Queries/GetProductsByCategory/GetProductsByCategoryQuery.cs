using MediatR;
using ErrorOr;
using SalesRecords.Products.Application.Common.Queries;
using SalesRecords.Products.Contracts.Dtos;

namespace SalesRecords.Products.Application.Queries.GetProductsByCategory;

public class GetProductsByCategoryQueryCriteria : QueryCriteria, IRequest<ErrorOr<List<ProductDto>>>
{
    public string Category { get; set; }

    public GetProductsByCategoryQueryCriteria(string category, int page, int size, string? order) 
        : base(page, size, order)
    {
        Category = category;
        Page = page;
        Size = size;
        Order = order;
    }
}