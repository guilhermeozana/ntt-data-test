using ErrorOr;
using MediatR;
using SalesRecords.Carts.Contracts.Dtos;

namespace SalesRecords.Carts.Application.Queries.GetCartById;

public class GetCartByIdQuery : IRequest<ErrorOr<CartDto>>
{
    public int Id { get; }

    public GetCartByIdQuery(int id)
    {
        Id = id;
    }
}