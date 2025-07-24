using DeveloperStore.Products.Domain.ValueObjects;
using DeveloperStore.Shared.SharedKernel.Domain;

namespace DeveloperStore.Products.Domain.Entities;

public class Product: AggregateRoot<int>
{
    public string Title { get; private set; }
    public decimal Price { get; private set; }
    public string Description { get; private set; }
    public string Category { get; private set; }
    public string Image { get; private set; }
    public Rating Rating { get; private set; }

    private Product(string title, decimal price, string description, string category, string image, Rating rating)
    {
        Title = title;
        Price = price;
        Description = description;
        Category = category;
        Image = image;
        Rating = rating;
    }
    
    private Product() { }

    public static Product Create(string title, decimal price, string description, string category, string image, Rating rating)
    {
        return new Product(title, price, description, category, image, rating);
    }

    public void Update(string title, decimal price, string description, string category, string image, Rating rating)
    {
        Title = title;
        Price = price;
        Description = description;
        Category = category;
        Image = image;
        Rating = rating;
    }
}