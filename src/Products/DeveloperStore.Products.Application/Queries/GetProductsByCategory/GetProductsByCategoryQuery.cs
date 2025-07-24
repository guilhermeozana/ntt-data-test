using MediatR;
using ErrorOr;
using DeveloperStore.Products.Contracts.Dtos;
using DeveloperStore.Shared.SharedKernel.Dtos;
using DeveloperStore.Shared.SharedKernel.Responses;

namespace DeveloperStore.Products.Application.Queries.GetProductsByCategory;

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