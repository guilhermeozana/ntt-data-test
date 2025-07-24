using DeveloperStore.Carts.Domain.Entities;
using DeveloperStore.Shared.SharedKernel.Dtos;
using DeveloperStore.Shared.SharedKernel.Pagination;

namespace DeveloperStore.Carts.Application.Common.Interfaces
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
