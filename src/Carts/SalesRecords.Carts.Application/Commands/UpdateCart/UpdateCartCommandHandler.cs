using AutoMapper;
using ErrorOr;
using MediatR;
using SalesRecords.Carts.Application.Common.Events;
using SalesRecords.Carts.Application.Common.Interfaces;
using SalesRecords.Carts.Contracts.Dtos;

namespace SalesRecords.Carts.Application.Commands.UpdateCart;

public class UpdateCartCommandHandler : IRequestHandler<UpdateCartCommand, ErrorOr<CartDto>>
{
    private readonly ICartRepository _repo;
    private readonly IMapper _mapper;
    private readonly IDomainEventDispatcher _dispatcher;

    public UpdateCartCommandHandler(ICartRepository repo, IMapper mapper, IDomainEventDispatcher dispatcher)
    {
        _repo = repo;
        _mapper = mapper;
        _dispatcher = dispatcher;
    }

    public async Task<ErrorOr<CartDto>> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
    {
        var existingCart = await _repo.GetByIdAsync(request.Id);

        if (existingCart is null)
        {
            return Error.NotFound("Cart.NotFound", "Cart not found.");
        }

        existingCart.Update(request.UserId, DateTime.Parse(request.Date));

        var updatedProductIds = request.Products.Select(p => p.ProductId).ToHashSet();

        var removedItems = existingCart.Items
            .Where(i => !updatedProductIds.Contains(i.ProductId) && !i.IsCancelled);

        foreach (var removed in removedItems)
        {
            existingCart.CancelItem(removed.ProductId);
        }

        foreach (var item in request.Products)
        {
            existingCart.AddItem(item.ProductId, item.Quantity);
        }

        await _repo.UpdateAsync(existingCart);
        
        await _dispatcher.DispatchEventsAsync(existingCart.DomainEvents);
        existingCart.ClearDomainEvents();
        
        var dto = _mapper.Map<CartDto>(existingCart);

        return dto;
    }
}