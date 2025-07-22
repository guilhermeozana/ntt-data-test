using SalesRecords.Products.Contracts.Dtos;
using SalesRecords.Products.Domain.Entities;
using SalesRecords.Shared.SharedKernel.Dtos;
using SalesRecords.Shared.SharedKernel.Pagination;

namespace SalesRecords.Products.Application.Common.Interfaces
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
