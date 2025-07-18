using ErrorOr;
using MediatR;
using SalesRecords.Products.Contracts.Dtos;

namespace SalesRecords.Products.Application.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<ErrorOr<ProductDto>>
{
    public Guid Id { get; }

    public GetProductByIdQuery(Guid id)
    {
        Id = id;
    }
}