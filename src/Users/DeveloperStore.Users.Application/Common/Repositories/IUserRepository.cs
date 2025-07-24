using DeveloperStore.Shared.SharedKernel.Dtos;
using DeveloperStore.Shared.SharedKernel.Pagination;
using DeveloperStore.Users.Domain.Entities;

namespace DeveloperStore.Users.Application.Common.Repositories
{
    public interface IUserRepository
    {
        Task<PagedResult<User>> GetAllAsync(QueryCriteriaDto queryCriteria);
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> DeleteAsync(User user);
    }
}
