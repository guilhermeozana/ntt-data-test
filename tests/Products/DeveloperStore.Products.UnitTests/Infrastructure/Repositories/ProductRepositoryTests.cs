using DeveloperStore.Products.Domain.Entities;
using DeveloperStore.Products.Domain.ValueObjects;
using DeveloperStore.Products.Infrastructure.Context;
using DeveloperStore.Products.Infrastructure.Repositories;
using DeveloperStore.Shared.SharedKernel.Dtos;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Products.UnitTests.Infrastructure.Repositories;

public class ProductRepositoryTests
{
    private ProductRepository GetRepository(out ProductsDbContext context)
    {
        var options = new DbContextOptionsBuilder<ProductsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new ProductsDbContext(options);
        context.Database.EnsureCreated();

        return new ProductRepository(context);
    }

    private static Product CreateProduct(string title = "Test Product", string category = "Books")
    {
        return Product.Create(
            title: title,
            price: 50.0m,
            description: "Sample description",
            category: category,
            image: "http://image.com/product.jpg",
            rating: new Rating(4.5, 20)
        );
    }

    [Fact]
    public async Task AddAsync_ShouldPersistProduct()
    {
        var repo = GetRepository(out var context);
        var product = CreateProduct();

        await repo.AddAsync(product);

        context.Products.Should().ContainSingle();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProduct()
    {
        var repo = GetRepository(out var context);
        var product = CreateProduct();
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        var retrieved = await repo.GetByIdAsync(product.Id);

        retrieved.Should().NotBeNull();
        retrieved!.Title.Should().Be(product.Title);
    }

    [Fact]
    public async Task UpdateAsync_ShouldChangeProductFields()
    {
        var repo = GetRepository(out var context);
        var product = CreateProduct();
        await repo.AddAsync(product);

        product.Update("Updated Title", 99.99m, "New Desc", "Electronics", "http://img.com", new Rating(4.9, 80));
        await repo.UpdateAsync(product);

        var updated = await repo.GetByIdAsync(product.Id);
        updated!.Title.Should().Be("Updated Title");
        updated.Category.Should().Be("Electronics");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveProduct()
    {
        var repo = GetRepository(out var context);
        var product = CreateProduct();
        await repo.AddAsync(product);

        var deleted = await repo.DeleteAsync(product);

        deleted.Should().BeTrue();
        context.Products.Should().BeEmpty();
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenProductExists()
    {
        var repo = GetRepository(out var context);
        var product = CreateProduct();
        await repo.AddAsync(product);

        var exists = await repo.ExistsAsync(product.Id);

        exists.Should().BeTrue();
    }

    [Fact]
    public async Task GetAllCategoriesAsync_ShouldReturnDistinctSortedCategories()
    {
        var repo = GetRepository(out var context);
        await context.Products.AddRangeAsync(
            CreateProduct("Book 1", "Books"),
            CreateProduct("Toy 1", "Toys"),
            CreateProduct("Book 2", "Books")
        );
        await context.SaveChangesAsync();

        var categories = await repo.GetAllCategoriesAsync();

        categories.Should().BeEquivalentTo(new[] { "Books", "Toys" }, opts => opts.WithStrictOrdering());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedProducts()
    {
        var repo = GetRepository(out var context);
        for (int i = 0; i < 12; i++)
        {
            await repo.AddAsync(CreateProduct(title: $"Product {i}", category: "General"));
        }

        var criteria = new QueryCriteriaDto(page: 2, size: 5, order: null, filters: null, minValues: null, maxValues: null);
        var result = await repo.GetAllAsync(criteria);

        result.Items.Should().HaveCount(5);
        result.TotalCount.Should().Be(12);
    }

    [Fact]
    public async Task GetByCategoryAsync_ShouldReturnProductsFilteredByCategory()
    {
        var repo = GetRepository(out var context);
        await repo.AddAsync(CreateProduct("Camera", "Electronics"));
        await repo.AddAsync(CreateProduct("Laptop", "Electronics"));
        await repo.AddAsync(CreateProduct("Chair", "Furniture"));

        var criteria = new QueryCriteriaDto(page: 1, size: 10, order: null, filters: null, minValues: null, maxValues: null);
        var result = await repo.GetByCategoryAsync("Electronics", criteria);

        result.Items.Should().HaveCount(2);
        result.Items.All(p => p.Category == "Electronics").Should().BeTrue();
    }
}