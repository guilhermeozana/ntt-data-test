using ErrorOr;
using MediatR;
using DeveloperStore.Sales.Contracts.Dtos;

namespace DeveloperStore.Sales.Application.Queries.GetSaleById;

public class GetSaleByIdQuery : IRequest<ErrorOr<SaleDto>>
{
    public int Id { get; }

    public GetSaleByIdQuery(int id)
    {
        Id = id;
    }
}