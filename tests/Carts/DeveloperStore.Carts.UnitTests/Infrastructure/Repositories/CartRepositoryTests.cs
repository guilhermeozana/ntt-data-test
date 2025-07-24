using DeveloperStore.Carts.Domain.Entities;
using DeveloperStore.Carts.Infrastructure.Context;
using DeveloperStore.Carts.Infrastructure.Repositories;
using DeveloperStore.Shared.SharedKernel.Dtos;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Carts.UnitTests.Infrastructure.Repositories;

public class CartRepositoryTests
{
    private static Cart CreateSampleCart(int userId = 1)
    {
        var cart = Cart.Create(userId);
        cart.AddItem(productId: 101, quantity: 2);
        cart.AddItem(productId: 202, quantity: 5);
        return cart;
    }

    private CartRepository GetRepository(out CartsDbContext context)
    {
        var options = new DbContextOptionsBuilder<CartsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        context = new CartsDbContext(options);
        context.Database.EnsureCreated();
        return new CartRepository(context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddCartSuccessfully()
    {
        var repo = GetRepository(out var context);
        var cart = CreateSampleCart();

        await repo.AddAsync(cart);

        context.Carts.Should().ContainSingle();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCartWithProducts()
    {
        var repo = GetRepository(out var context);
        var cart = CreateSampleCart();
        await context.Carts.AddAsync(cart);
        await context.SaveChangesAsync();

        var retrieved = await repo.GetByIdAsync(cart.Id);

        retrieved.Should().NotBeNull();
        retrieved!.Products.Should().HaveCount(2);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenCartExists()
    {
        var repo = GetRepository(out var context);
        var cart = CreateSampleCart();
        await context.Carts.AddAsync(cart);
        await context.SaveChangesAsync();

        var exists = await repo.ExistsAsync(cart.Id);

        exists.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistChanges()
    {
        var repo = GetRepository(out var context);
        var cart = CreateSampleCart(userId: 10);
        await context.Carts.AddAsync(cart);
        await context.SaveChangesAsync();

        cart.Update(userId: 99, DateTime.UtcNow);
        await repo.UpdateAsync(cart);

        var updated = await repo.GetByIdAsync(cart.Id);
        updated!.UserId.Should().Be(99);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveCart()
    {
        var repo = GetRepository(out var context);
        var cart = CreateSampleCart();
        await context.Carts.AddAsync(cart);
        await context.SaveChangesAsync();

        var deleted = await repo.DeleteAsync(cart);

        deleted.Should().BeTrue();
        context.Carts.Should().BeEmpty();
    }

    [Fact]
    public async Task GetPagedAsync_ShouldRespectPagination()
    {
        var repo = GetRepository(out var context);
        for (int i = 0; i < 15; i++)
        {
            await context.Carts.AddAsync(Cart.Create(userId: i));
        }
        await context.SaveChangesAsync();

        var criteria = new QueryCriteriaDto(page: 2, size: 5, order: null, filters: null, minValues: null, maxValues: null);
        var result = await repo.GetPagedAsync(criteria);

        result.Items.Should().HaveCount(5);
        result.TotalCount.Should().Be(15);
    }
}