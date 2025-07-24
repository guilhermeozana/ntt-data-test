using ErrorOr;
using MediatR;
using DeveloperStore.Carts.Contracts.Dtos;

namespace DeveloperStore.Carts.Application.Queries.GetCartById;

public class GetCartByIdQuery : IRequest<ErrorOr<CartDto>>
{
    public int Id { get; }

    public GetCartByIdQuery(int id)
    {
        Id = id;
    }
}