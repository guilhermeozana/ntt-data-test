using MediatR;
using ErrorOr;

namespace DeveloperStore.Products.Application.Commands.DeleteProduct;

public class DeleteProductCommand : IRequest<ErrorOr<Deleted>>
{
    public int Id { get; }

    public DeleteProductCommand(int id)
    {
        Id = id;
    }
}