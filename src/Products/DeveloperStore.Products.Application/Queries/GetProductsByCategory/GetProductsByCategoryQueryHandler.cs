using AutoMapper;
using ErrorOr;
using MediatR;
using DeveloperStore.Products.Application.Common.Interfaces;
using DeveloperStore.Products.Contracts.Dtos;
using DeveloperStore.Shared.SharedKernel.Responses;

namespace DeveloperStore.Products.Application.Queries.GetProductsByCategory;

public class GetProductsByCategoryQueryHandler : IRequestHandler<GetProductsByCategoryQuery, ErrorOr<PagedResponse<ProductDto>>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public GetProductsByCategoryQueryHandler(IProductRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ErrorOr<PagedResponse<ProductDto>>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var result = await _repo.GetByCategoryAsync(request.Category, request.Criteria);

        var productDtos = _mapper.Map<List<ProductDto>>(result.Items);

        var totalPages = (int)Math.Ceiling((double)result.TotalCount / request.Criteria.Size);

        var pagedResponse = new PagedResponse<ProductDto>
        {
            Data = productDtos,
            TotalItems = result.TotalCount,
            CurrentPage = request.Criteria.Page,
            TotalPages = totalPages
        };

        return pagedResponse;
    }
}