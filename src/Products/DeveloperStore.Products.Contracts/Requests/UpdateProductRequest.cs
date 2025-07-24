using DeveloperStore.Products.Contracts.Dtos;

namespace DeveloperStore.Products.Contracts.Requests;

public class UpdateProductRequest
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Image { get; set; }
    public RatingDto Rating { get; set; }
}