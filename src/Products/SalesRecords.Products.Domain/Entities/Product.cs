using SalesRecords.Products.Domain.ValueObjects;

namespace SalesRecords.Products.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public decimal Price { get; private set; }
    public string Description { get; private set; }
    public string Category { get; private set; }
    public string Image { get; private set; }
    public Rating Rating { get; private set; }

    private Product(Guid id, string title, decimal price, string description, string category, string image, Rating rating)
    {
        Id = id;
        Title = title;
        Price = price;
        Description = description;
        Category = category;
        Image = image;
        Rating = rating;
    }

    public static Product Create(string title, decimal price, string description, string category, string image, Rating rating)
    {
        return new Product(Guid.NewGuid(), title, price, description, category, image, rating);
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