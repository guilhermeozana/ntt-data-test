using Microsoft.EntityFrameworkCore;
using DeveloperStore.Carts.Application.Common.Interfaces;
using DeveloperStore.Carts.Domain.Entities;
using DeveloperStore.Carts.Infrastructure.Context;
using DeveloperStore.Shared.SharedKernel.Dtos;
using DeveloperStore.Shared.SharedKernel.Extensions;
using DeveloperStore.Shared.SharedKernel.Pagination;

namespace DeveloperStore.Carts.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly CartsDbContext _context;

    public CartRepository(CartsDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<Cart>> GetPagedAsync(QueryCriteriaDto criteria)
    {
        IQueryable<Cart> filteredQuery = _context.Carts
            .Include(c => c.Products)
            .ApplyFiltering(criteria.Filters, criteria.MinValues, criteria.MaxValues);

        var totalCount = await filteredQuery.CountAsync();

        var pagedQuery = filteredQuery
            .ApplyOrdering(criteria.Order)
            .ApplyPagination(criteria.Page, criteria.Size);

        var items = await pagedQuery.ToListAsync();

        return new PagedResult<Cart>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    public async Task<Cart?> GetByIdAsync(int id)
    {
        return await _context.Carts
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Cart cart)
    {
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Cart cart)
    {
        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Carts.AnyAsync(c => c.Id == id);
    }
}