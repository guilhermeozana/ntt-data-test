using DeveloperStore.Products.Contracts.Dtos;
using DeveloperStore.Products.Domain.Entities;
using DeveloperStore.Shared.SharedKernel.Dtos;
using DeveloperStore.Shared.SharedKernel.Pagination;

namespace DeveloperStore.Products.Application.Common.Interfaces
{
    public interface IProductRepository
    {
        Task<PagedResult<Product>> GetAllAsync(QueryCriteriaDto queryCriteria);
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task<bool> DeleteAsync(Product product);
        Task<List<string>> GetAllCategoriesAsync();
        Task<PagedResult<Product>> GetByCategoryAsync(string category, QueryCriteriaDto queryCriteria);
        Task<bool> ExistsAsync(int id);
    }
}
