using ErrorOr;
using MediatR;
using SalesRecords.Carts.Contracts.Dtos;
using SalesRecords.Shared.SharedKernel.Dtos;
using SalesRecords.Shared.SharedKernel.Responses;

namespace SalesRecords.Carts.Application.Queries.GetAllCarts;

public class GetAllCartsQuery : IRequest<ErrorOr<PagedResponse<CartDto>>>
{
    public QueryCriteriaDto Criteria { get; }

    public GetAllCartsQuery(QueryCriteriaDto criteria)
    {
        Criteria = criteria;
    }
}