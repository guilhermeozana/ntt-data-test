using Microsoft.EntityFrameworkCore;
using SalesRecords.Products.Application.Common.Interfaces;
using SalesRecords.Products.Domain.Entities;
using SalesRecords.Products.Infrastructure.Context;

namespace SalesRecords.Products.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductsDbContext _context;

        public ProductRepository(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<List<Product>> GetByCategoryAsync(string category, int? limit)
        {
            var query = _context.Products
                .Where(p => p.Category == category);

            if (limit.HasValue)
                query = query.Take(limit.Value);

            return await query
                .OrderBy(p => p.Id)
                .ToListAsync();
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

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Products.AnyAsync(p => p.Id.Equals(id));
        }
    }
}
