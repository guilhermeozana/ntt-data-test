using AutoMapper;
using DeveloperStore.Products.Application.Common.Interfaces;
using DeveloperStore.Products.Contracts.Dtos;
using ErrorOr;
using MediatR;

namespace DeveloperStore.Products.Application.Queries.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ErrorOr<ProductDto>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IProductRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ErrorOr<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repo.GetByIdAsync(request.Id);
        if (product == null)
            return Error.NotFound("Product.NotFound","Product not found.");
        
        var productDto = _mapper.Map<ProductDto>(product);

        return productDto;
    }
}