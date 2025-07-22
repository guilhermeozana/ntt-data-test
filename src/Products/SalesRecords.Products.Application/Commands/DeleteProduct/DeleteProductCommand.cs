using MediatR;
using ErrorOr;

namespace SalesRecords.Products.Application.Commands.DeleteProduct;

public class DeleteProductCommand : IRequest<ErrorOr<Deleted>>
{
    public int Id { get; }

    public DeleteProductCommand(int id)
    {
        Id = id;
    }
}