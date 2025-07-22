using MediatR;
using ErrorOr;
using SalesRecords.Products.Contracts.Dtos;
using SalesRecords.Shared.SharedKernel.Dtos;
using SalesRecords.Shared.SharedKernel.Responses;

namespace SalesRecords.Products.Application.Queries.GetProductsByCategory;

public class GetProductsByCategoryQuery : IRequest<ErrorOr<PagedResponse<ProductDto>>>
{
    public GetProductsByCategoryQuery(string category, QueryCriteriaDto criteria)
    {
        Category = category;
        Criteria = criteria;
    }

    public string Category { get; set; }
    
    public QueryCriteriaDto Criteria { get; set; }
}