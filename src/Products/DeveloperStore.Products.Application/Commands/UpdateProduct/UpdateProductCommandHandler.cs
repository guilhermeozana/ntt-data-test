using AutoMapper;
using ErrorOr;
using MediatR;
using DeveloperStore.Products.Application.Common.Interfaces;
using DeveloperStore.Products.Contracts.Dtos;
using DeveloperStore.Products.Domain.Entities;
using DeveloperStore.Products.Domain.ValueObjects;

namespace DeveloperStore.Products.Application.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ErrorOr<ProductDto>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(IProductRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ErrorOr<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var existingProduct = await _repo.GetByIdAsync(request.Id);
        
        if (existingProduct is null)
        {
            return Error.NotFound("Product.Update.NotFound", $"Product with id {request.Id} not found");
        }

        existingProduct.Update(
            request.Title,
            request.Price,
            request.Description,
            request.Category,
            request.Image,
            new Rating(request.Rating.Rate, request.Rating.Count)
        );

        await _repo.UpdateAsync(existingProduct);
        var productDto = _mapper.Map<ProductDto>(existingProduct);

        return productDto;
    }

}