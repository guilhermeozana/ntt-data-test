using AutoMapper;
using ErrorOr;
using MediatR;
using DeveloperStore.Carts.Application.Common.Interfaces;
using DeveloperStore.Carts.Contracts.Dtos;

namespace DeveloperStore.Carts.Application.Queries.GetCartById;

public class GetCartByIdQueryHandler : IRequestHandler<GetCartByIdQuery, ErrorOr<CartDto>>
{
    private readonly ICartRepository _repo;
    private readonly IMapper _mapper;

    public GetCartByIdQueryHandler(ICartRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ErrorOr<CartDto>> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
    {
        var cart = await _repo.GetByIdAsync(request.Id);
        if (cart == null)
            return Error.NotFound("Cart not found.");
        
        var cartDto = _mapper.Map<CartDto>(cart);

        return cartDto;
    }
}