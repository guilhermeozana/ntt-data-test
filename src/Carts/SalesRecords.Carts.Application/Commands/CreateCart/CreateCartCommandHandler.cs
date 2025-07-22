using AutoMapper;
using ErrorOr;
using MediatR;
using SalesRecords.Carts.Application.Common.Events;
using SalesRecords.Carts.Application.Common.Interfaces;
using SalesRecords.Carts.Contracts.Dtos;
using SalesRecords.Carts.Domain.Entities;
using SalesRecords.Carts.Domain.Events;
using SalesRecords.Shared.SharedKernel.Domain.Events;

namespace SalesRecords.Carts.Application.Commands.CreateCart;

public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, ErrorOr<CartDto>>
{
    private readonly ICartRepository _repo;
    private readonly IMapper _mapper;
    private readonly IDomainEventDispatcher _dispatcher;

    public CreateCartCommandHandler(
        ICartRepository repo,
        IMapper mapper,
        IDomainEventDispatcher dispatcher)
    {
        _repo = repo;
        _mapper = mapper;
        _dispatcher = dispatcher;
    }

    public async Task<ErrorOr<CartDto>> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        var cart = Cart.Create(request.UserId, DateTime.Parse(request.Date));

        foreach (var item in request.Products)
        {
            cart.AddItem(item.ProductId, item.Quantity);
        }

        await _repo.AddAsync(cart);

        await _dispatcher.DispatchEventsAsync(cart.DomainEvents);
        cart.ClearDomainEvents();

        var cartDto = _mapper.Map<CartDto>(cart);

        return cartDto;
    }
}