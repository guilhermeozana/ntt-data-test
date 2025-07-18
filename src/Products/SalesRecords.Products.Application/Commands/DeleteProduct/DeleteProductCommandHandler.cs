using MediatR;
using SalesRecords.Products.Application.Common.Interfaces;

namespace SalesRecords.Products.Application.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _repo;

    public DeleteProductCommandHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repo.GetByIdAsync(request.Id);

        if (product == null)
        {
            throw new ArgumentException($"Product with id {request.Id} does not exist");
        }
        
        return await _repo.DeleteAsync(product);
    }
}