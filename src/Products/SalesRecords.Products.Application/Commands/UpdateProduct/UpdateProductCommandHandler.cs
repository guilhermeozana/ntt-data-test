using AutoMapper;
using ErrorOr;
using MediatR;
using SalesRecords.Products.Api.Contracts.Dtos;
using SalesRecords.Products.Application.Common.Interfaces;
using SalesRecords.Products.Contracts.Dtos;
using SalesRecords.Products.Domain.Entities;
using SalesRecords.Products.Domain.ValueObjects;

namespace SalesRecords.Products.Application.Commands.UpdateProduct;

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
            Error.NotFound("Product not found");
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