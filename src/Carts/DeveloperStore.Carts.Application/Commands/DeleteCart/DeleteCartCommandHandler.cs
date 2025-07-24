using ErrorOr;
using MediatR;
using DeveloperStore.Carts.Application.Common.Interfaces;

namespace DeveloperStore.Carts.Application.Commands.DeleteCart;

public class DeleteCartCommandHandler : IRequestHandler<DeleteCartCommand, ErrorOr<Deleted>>
{
    private readonly ICartRepository _repo;

    public DeleteCartCommandHandler(ICartRepository repo)
    {
        _repo = repo;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _repo.GetByIdAsync(request.Id);

        if (cart is null)
        {
            return Error.NotFound("Cart.NotFound", $"Cart with id {request.Id} was not found.");
        }

        await _repo.DeleteAsync(cart);

        return Result.Deleted;
    }
}