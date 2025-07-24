using ErrorOr;
using MediatR;
using DeveloperStore.Carts.Contracts.Dtos;
using DeveloperStore.Shared.SharedKernel.Dtos;
using DeveloperStore.Shared.SharedKernel.Responses;

namespace DeveloperStore.Carts.Application.Queries.GetAllCarts;

public class GetAllCartsQuery : IRequest<ErrorOr<PagedResponse<CartDto>>>
{
    public QueryCriteriaDto Criteria { get; }

    public GetAllCartsQuery(QueryCriteriaDto criteria)
    {
        Criteria = criteria;
    }
}