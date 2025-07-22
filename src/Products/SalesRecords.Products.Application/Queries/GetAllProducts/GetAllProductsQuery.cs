using MediatR;
using ErrorOr;
using SalesRecords.Products.Contracts.Dtos;
using SalesRecords.Shared.SharedKernel.Dtos;
using SalesRecords.Shared.SharedKernel.Responses;

namespace SalesRecords.Products.Application.Queries.GetAllProducts;

public class GetAllProductsQuery : IRequest<ErrorOr<PagedResponse<ProductDto>>>
{
    public QueryCriteriaDto Criteria { get; }

    public GetAllProductsQuery(QueryCriteriaDto criteria)
    {
        Criteria = criteria;
    }
}