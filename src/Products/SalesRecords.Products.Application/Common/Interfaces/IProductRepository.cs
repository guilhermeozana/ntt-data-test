using SalesRecords.Products.Domain.Entities;

namespace SalesRecords.Products.Application.Common.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(Guid id);
        Task<List<Product>> GetByCategoryAsync(string category, int? limit);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task<bool> DeleteAsync(Product product);
        Task<bool> ExistsAsync(Guid id);
    }
}
