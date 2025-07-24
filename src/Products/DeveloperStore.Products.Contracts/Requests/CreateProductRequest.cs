using DeveloperStore.Products.Contracts.Dtos;

namespace DeveloperStore.Products.Contracts.Requests;

public class CreateProductRequest
{
    public required string Title { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public required string Category { get; set; }
    public string? Image { get; set; }
    public RatingDto? Rating { get; set; }
}
