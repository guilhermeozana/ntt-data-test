using ErrorOr;
using MediatR;
using DeveloperStore.Products.Contracts.Dtos;

namespace DeveloperStore.Products.Application.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<ErrorOr<ProductDto>>
{
    public int Id { get; }

    public GetProductByIdQuery(int id)
    {
        Id = id;
    }
}