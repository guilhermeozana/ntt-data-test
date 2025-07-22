using ErrorOr;
using MediatR;
using SalesRecords.Products.Contracts.Dtos;

namespace SalesRecords.Products.Application.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<ErrorOr<ProductDto>>
{
    public int Id { get; }

    public GetProductByIdQuery(int id)
    {
        Id = id;
    }
}