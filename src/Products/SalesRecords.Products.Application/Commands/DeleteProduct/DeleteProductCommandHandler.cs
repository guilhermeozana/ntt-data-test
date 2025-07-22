using MediatR;
using ErrorOr;
using SalesRecords.Products.Application.Common.Interfaces;

namespace SalesRecords.Products.Application.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ErrorOr<Deleted>>
{
    private readonly IProductRepository _repo;

    public DeleteProductCommandHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repo.GetByIdAsync(request.Id);

        if (product == null)
        {
            throw new ArgumentException($"Product with id {request.Id} does not exist");
        }
        
        var deleted = await _repo.DeleteAsync(product);
        
        return deleted ? Result.Deleted : Error.Unexpected("Failed to delete product");
    }
}