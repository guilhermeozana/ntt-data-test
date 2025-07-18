using ErrorOr;
using MediatR;
using SalesRecords.Products.Api.Contracts.Dtos;
using SalesRecords.Products.Contracts.Dtos;

namespace SalesRecords.Products.Application.Commands.UpdateProduct;

public class UpdateProductCommand : IRequest<ErrorOr<ProductDto>>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Image { get; set; }
    public RatingDto Rating { get; set; }
}