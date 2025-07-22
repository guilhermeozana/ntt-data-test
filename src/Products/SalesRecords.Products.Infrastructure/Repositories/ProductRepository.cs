using Microsoft.EntityFrameworkCore;
using SalesRecords.Products.Application.Common.Interfaces;
using SalesRecords.Products.Domain.Entities;
using SalesRecords.Products.Infrastructure.Context;
using SalesRecords.Shared.SharedKernel.Dtos;
using SalesRecords.Shared.SharedKernel.Extensions;
using SalesRecords.Shared.SharedKernel.Pagination;

namespace SalesRecords.Products.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductsDbContext _context;

        public ProductRepository(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Product>> GetAllAsync(QueryCriteriaDto queryCriteria)
        {
            IQueryable<Product> filteredQuery = _context.Products
                .ApplyFiltering(queryCriteria.Filters, queryCriteria.MinValues, queryCriteria.MaxValues);

            var totalCount = await filteredQuery.CountAsync();

            var pagedItems = await filteredQuery
                .ApplyOrdering(queryCriteria.Order)
                .ApplyPagination(queryCriteria.Page, queryCriteria.Size)
                .ToListAsync();

            return new PagedResult<Product>
            {
                Items = pagedItems,
                TotalCount = totalCount
            };
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            
            return true;
        }
        
        public async Task<List<string>> GetAllCategoriesAsync()
        {
            return await _context.Products
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }
        
        public async Task<PagedResult<Product>> GetByCategoryAsync(string category, QueryCriteriaDto queryCriteria)
        {
            var baseQuery = _context.Products
                .Where(p => p.Category == category)
                .ApplyFiltering(queryCriteria.Filters, queryCriteria.MinValues, queryCriteria.MaxValues);

            var totalCount = await baseQuery.CountAsync();

            var pagedQuery = baseQuery
                .ApplyOrdering(queryCriteria.Order)
                .ApplyPagination(queryCriteria.Page, queryCriteria.Size);

            var products = await pagedQuery.ToListAsync();

            return new PagedResult<Product>
            {
                Items = products,
                TotalCount = totalCount
            };
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(p => p.Id.Equals(id));
        }
    }
}
