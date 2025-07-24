using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Infrastructure.Context;
using DeveloperStore.Sales.Infrastructure.Repositories;
using DeveloperStore.Shared.SharedKernel.Dtos;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Sales.UnitTests.Infrastructure.Repositories;

public class SaleRepositoryTests
{
    private static Sale CreateSale(int customerId = 1, string number = "S-001", string category = "Default")
    {
        var sale = Sale.Create(number, DateTime.UtcNow, customerId, branchId: 99);
        sale.AddItem(productId: 101, quantity: 2, unitPrice: 50);
        sale.AddItem(productId: 202, quantity: 1, unitPrice: 75);
        return sale;
    }

    private SaleRepository GetRepository(out SalesDbContext context)
    {
        var options = new DbContextOptionsBuilder<SalesDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        context = new SalesDbContext(options);
        context.Database.EnsureCreated();

        return new SaleRepository(context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddSaleSuccessfully()
    {
        var repo = GetRepository(out var context);
        var sale = CreateSale();

        await repo.AddAsync(sale);

        context.Sales.Should().ContainSingle();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSaleWithProducts()
    {
        var repo = GetRepository(out var context);
        var sale = CreateSale();
        await context.Sales.AddAsync(sale);
        await context.SaveChangesAsync();

        var retrieved = await repo.GetByIdAsync(sale.Id);

        retrieved.Should().NotBeNull();
        retrieved!.Products.Should().HaveCount(2);
        retrieved.TotalAmount.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task UpdateAsync_ShouldApplyChangesToSale()
    {
        var repo = GetRepository(out var context);
        var sale = CreateSale(customerId: 10);
        await repo.AddAsync(sale);

        var newDate = DateTime.UtcNow.AddDays(1);
        sale.Update(newDate, newCustomerId: 20, newBranchId: 77);
        await repo.UpdateAsync(sale);

        var updated = await repo.GetByIdAsync(sale.Id);
        updated!.CustomerId.Should().Be(20);
        updated.BranchId.Should().Be(77);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenSaleExists()
    {
        var repo = GetRepository(out var context);
        var sale = CreateSale();
        await repo.AddAsync(sale);

        var exists = await repo.ExistsAsync(sale.Id);

        exists.Should().BeTrue();
    }

    [Fact]
    public async Task GetPagedAsync_ShouldReturnCorrectPage()
    {
        var repo = GetRepository(out var context);

        for (int i = 0; i < 12; i++)
        {
            await repo.AddAsync(Sale.Create($"S-{i:D3}", DateTime.UtcNow, customerId: i, branchId: 1));
        }

        var criteria = new QueryCriteriaDto(page: 2, size: 5, order: null, filters: null, minValues: null, maxValues: null);
        var result = await repo.GetPagedAsync(criteria);

        result.Items.Should().HaveCount(5);
        result.TotalCount.Should().Be(12);
    }
}