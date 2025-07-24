using MediatR;
using ErrorOr;
using DeveloperStore.Products.Contracts.Dtos;
using DeveloperStore.Shared.SharedKernel.Dtos;
using DeveloperStore.Shared.SharedKernel.Responses;

namespace DeveloperStore.Products.Application.Queries.GetAllProducts;

public class GetAllProductsQuery : IRequest<ErrorOr<PagedResponse<ProductDto>>>
{
    public QueryCriteriaDto Criteria { get; }

    public GetAllProductsQuery(QueryCriteriaDto criteria)
    {
        Criteria = criteria;
    }
}