using SalesRecords.Products.Api.Contracts.Dtos;

namespace SalesRecords.Products.Api.Contracts;

public class CreateProductRequest
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Image { get; set; }
    public RatingDto Rating { get; set; }
}
