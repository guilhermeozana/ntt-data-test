using ErrorOr;
using MediatR;
using DeveloperStore.Products.Contracts.Dtos;

namespace DeveloperStore.Products.Application.Commands.CreateProduct;

public class CreateProductCommand : IRequest<ErrorOr<ProductDto>>
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public decimal Price { get; set; }
    public required string Description { get; set; }
    public required string Category { get; set; }
    public string? Image { get; set; }
    public RatingDto? Rating { get; set; }
}