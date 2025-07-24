using Microsoft.EntityFrameworkCore;
using DeveloperStore.Sales.Application.Common.Repositories;
using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Sales.Infrastructure.Context;
using DeveloperStore.Shared.SharedKernel.Dtos;
using DeveloperStore.Shared.SharedKernel.Extensions;
using DeveloperStore.Shared.SharedKernel.Pagination;

namespace DeveloperStore.Sales.Infrastructure.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly SalesDbContext _context;

    public SaleRepository(SalesDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<Sale>> GetPagedAsync(QueryCriteriaDto criteria)
    {
        IQueryable<Sale> query = _context.Sales
            .Include(s => s.Products)
            .ApplyFiltering(criteria.Filters, criteria.MinValues, criteria.MaxValues);

        var totalCount = await query.CountAsync();

        var pagedQuery = query
            .ApplyOrdering(criteria.Order)
            .ApplyPagination(criteria.Page, criteria.Size);

        var items = await pagedQuery.ToListAsync();

        return new PagedResult<Sale>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    public async Task<Sale?> GetByIdAsync(int id)
    {
        return await _context.Sales
            .Include(s => s.Products)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task AddAsync(Sale sale)
    {
        await _context.Sales.AddAsync(sale);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Sale sale)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Sales.AnyAsync(s => s.Id == id);
    }
}