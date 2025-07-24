using AutoMapper;
using ErrorOr;
using MediatR;
using DeveloperStore.Carts.Application.Common.Interfaces;
using DeveloperStore.Carts.Contracts.Dtos;

namespace DeveloperStore.Carts.Application.Commands.UpdateCart;

public class UpdateCartCommandHandler : IRequestHandler<UpdateCartCommand, ErrorOr<CartDto>>
{
    private readonly ICartRepository _repo;
    private readonly IMapper _mapper;

    public UpdateCartCommandHandler(ICartRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ErrorOr<CartDto>> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _repo.GetByIdAsync(request.Id);
        if (cart is null)
            return Error.NotFound("Cart.NotFound", "Cart not found.");

        cart.Update(request.UserId, DateTime.Parse(request.Date).ToUniversalTime());

        cart.ClearItems();
        foreach (var product in request.Products)
        {
            var result = cart.AddItem(product.ProductId, product.Quantity);
            if (result.IsError)
                return result.Errors;
        }

        await _repo.UpdateAsync(cart);

        var dto = _mapper.Map<CartDto>(cart);
        return dto;
    }
}