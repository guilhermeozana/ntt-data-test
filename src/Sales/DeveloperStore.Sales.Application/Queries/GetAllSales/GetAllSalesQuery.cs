using ErrorOr;
using MediatR;
using DeveloperStore.Sales.Contracts.Dtos;
using DeveloperStore.Shared.SharedKernel.Dtos;
using DeveloperStore.Shared.SharedKernel.Responses;

namespace DeveloperStore.Sales.Application.Queries.GetAllSales;

public class GetAllSalesQuery : IRequest<ErrorOr<PagedResponse<SaleDto>>>
{
    public QueryCriteriaDto Criteria { get; }

    public GetAllSalesQuery(QueryCriteriaDto criteria)
    {
        Criteria = criteria;
    }
}