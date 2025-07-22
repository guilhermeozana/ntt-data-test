using AutoMapper;
using ErrorOr;
using MediatR;
using SalesRecords.Products.Application.Common.Interfaces;
using SalesRecords.Products.Contracts.Dtos;
using SalesRecords.Products.Domain.Entities;
using SalesRecords.Products.Domain.ValueObjects;

namespace SalesRecords.Products.Application.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ErrorOr<ProductDto>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IProductRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ErrorOr<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Product.Create(
            request.Title,
            request.Price,
            request.Description,
            request.Category,
            request.Image,
            new Rating(0, 0)
        );

        await _repo.AddAsync(product);

        var productDto = _mapper.Map<ProductDto>(product);
        
        return productDto;
    }
}