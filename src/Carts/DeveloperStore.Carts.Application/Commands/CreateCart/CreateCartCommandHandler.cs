using AutoMapper;
using ErrorOr;
using MediatR;
using DeveloperStore.Carts.Application.Common.Interfaces;
using DeveloperStore.Carts.Contracts.Dtos;
using DeveloperStore.Carts.Domain.Entities;

namespace DeveloperStore.Carts.Application.Commands.CreateCart;

public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, ErrorOr<CartDto>>
{
    private readonly ICartRepository _repo;
    private readonly IMapper _mapper;

    public CreateCartCommandHandler(
        ICartRepository repo,
        IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ErrorOr<CartDto>> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        var cart = Cart.Create(request.UserId, DateTime.Parse(request.Date).ToUniversalTime());

        foreach (var item in request.Products)
        {
            cart.AddItem(item.ProductId, item.Quantity);
        }

        await _repo.AddAsync(cart);

        var cartDto = _mapper.Map<CartDto>(cart);

        return cartDto;
    }
}