using AutoMapper;
using ErrorOr;
using MediatR;
using DeveloperStore.Products.Application.Common.Interfaces;
using DeveloperStore.Products.Contracts.Dtos;
using DeveloperStore.Products.Domain.Entities;
using DeveloperStore.Products.Domain.ValueObjects;

namespace DeveloperStore.Products.Application.Commands.CreateProduct;

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
        var rating = new Rating(rate: request.Rating.Rate, count: request.Rating.Count);
        
        var product = Product.Create(
            request.Title,
            request.Price,
            request.Description,
            request.Category,
            request.Image,
            rating
        );

        await _repo.AddAsync(product);

        var productDto = _mapper.Map<ProductDto>(product);
        
        return productDto;
    }
}