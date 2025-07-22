using ErrorOr;
using MediatR;
using SalesRecords.Carts.Application.Common.Events;
using SalesRecords.Carts.Application.Common.Interfaces;

namespace SalesRecords.Carts.Application.Commands.DeleteCart;

public class DeleteCartCommandHandler : IRequestHandler<DeleteCartCommand, ErrorOr<Deleted>>
{
    private readonly ICartRepository _repo;
    private readonly IDomainEventDispatcher _dispatcher;

    public DeleteCartCommandHandler(ICartRepository repo, IDomainEventDispatcher dispatcher)
    {
        _repo = repo;
        _dispatcher = dispatcher;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _repo.GetByIdAsync(request.Id);

        if (cart is null)
        {
            return Error.NotFound("Cart.NotFound", $"Cart with id {request.Id} was not found.");
        }

        cart.Cancel();

        await _repo.UpdateAsync(cart);
        
        await _dispatcher.DispatchEventsAsync(cart.DomainEvents);
        cart.ClearDomainEvents();

        return Result.Deleted;
    }
}