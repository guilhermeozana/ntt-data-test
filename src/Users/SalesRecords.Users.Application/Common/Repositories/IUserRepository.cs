using SalesRecords.Shared.SharedKernel.Dtos;
using SalesRecords.Shared.SharedKernel.Pagination;
using SalesRecords.Users.Domain.Entities;

namespace SalesRecords.Users.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<PagedResult<User>> GetAllAsync(QueryCriteriaDto queryCriteria);
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> DeleteAsync(User user);
    }
}
