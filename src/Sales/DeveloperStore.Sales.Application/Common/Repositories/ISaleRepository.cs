using DeveloperStore.Sales.Domain.Entities;
using DeveloperStore.Shared.SharedKernel.Dtos;
using DeveloperStore.Shared.SharedKernel.Pagination;

namespace DeveloperStore.Sales.Application.Common.Repositories
{
    public interface ISaleRepository
    {
        Task<PagedResult<Sale>> GetPagedAsync(QueryCriteriaDto queryCriteria);
        Task<Sale?> GetByIdAsync(int id);
        Task AddAsync(Sale sale);
        Task UpdateAsync(Sale sale);
        Task<bool> ExistsAsync(int id);
    }
}