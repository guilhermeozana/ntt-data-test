using AutoMapper;
using ErrorOr;
using MediatR;
using SalesRecords.Products.Application.Common.Interfaces;
using SalesRecords.Products.Contracts.Dtos;
using SalesRecords.Shared.SharedKernel.Responses;

namespace SalesRecords.Products.Application.Queries.GetAllProducts;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, ErrorOr<PagedResponse<ProductDto>>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public GetAllProductsQueryHandler(IProductRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ErrorOr<PagedResponse<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var result = await _repo.GetAllAsync(request.Criteria); // Retorna PagedResult<Product>

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