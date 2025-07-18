using ErrorOr;
using AutoMapper;
using MediatR;
using SalesRecords.Products.Application.Common.Interfaces;
using SalesRecords.Products.Application.Queries.GetProductById;
using SalesRecords.Products.Contracts.Dtos;

namespace SalesRecords.Products.Application.Queries.GetProductsByCategory;

public class GetProductsByCategoryQueryHandler : IRequestHandler<GetProductByIdQuery, ErrorOr<ProductDto>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public GetProductsByCategoryQueryHandler(IProductRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ErrorOr<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repo.GetByIdAsync(request.Id);
        if (product == null)
            throw new KeyNotFoundException("Product not found.");
        
        var productDto = _mapper.Map<ProductDto>(product);

        return productDto;
    }
}