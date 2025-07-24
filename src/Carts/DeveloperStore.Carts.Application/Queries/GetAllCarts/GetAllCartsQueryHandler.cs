using AutoMapper;
using ErrorOr;
using MediatR;
using DeveloperStore.Carts.Application.Common.Interfaces;
using DeveloperStore.Carts.Contracts.Dtos;
using DeveloperStore.Shared.SharedKernel.Responses;

namespace DeveloperStore.Carts.Application.Queries.GetAllCarts;

public class GetAllCartsQueryHandler : IRequestHandler<GetAllCartsQuery, ErrorOr<PagedResponse<CartDto>>>
{
    private readonly ICartRepository _repo;
    private readonly IMapper _mapper;

    public GetAllCartsQueryHandler(ICartRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ErrorOr<PagedResponse<CartDto>>> Handle(GetAllCartsQuery request, CancellationToken cancellationToken)
    {
        var result = await _repo.GetPagedAsync(request.Criteria);

        var cartDtos = _mapper.Map<List<CartDto>>(result.Items);

        var totalPages = (int)Math.Ceiling((double)result.TotalCount / request.Criteria.Size);

        var pagedResponse = new PagedResponse<CartDto>
        {
            Data = cartDtos,
            TotalItems = result.TotalCount,
            CurrentPage = request.Criteria.Page,
            TotalPages = totalPages
        };

        return pagedResponse;
    }
}