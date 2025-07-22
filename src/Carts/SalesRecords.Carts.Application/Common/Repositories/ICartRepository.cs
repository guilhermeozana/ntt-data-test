using SalesRecords.Carts.Domain.Entities;
using SalesRecords.Shared.SharedKernel.Dtos;
using SalesRecords.Shared.SharedKernel.Pagination;

namespace SalesRecords.Carts.Application.Common.Interfaces
{
    public interface ICartRepository
    {
        Task<PagedResult<Cart>> GetPagedAsync(QueryCriteriaDto queryCriteria);
        Task<Cart?> GetByIdAsync(int id);
        Task AddAsync(Cart cart);
        Task UpdateAsync(Cart cart);
        Task<bool> DeleteAsync(Cart cart);
        
        Task<bool> ExistsAsync(int id);
    }
}
